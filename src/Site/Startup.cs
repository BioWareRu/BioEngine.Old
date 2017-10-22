using System;
using System.Globalization;
using System.Linq;
using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Ipb;
using BioEngine.Routing;
using BioEngine.Site.Components;
using cloudscribe.Syndication.Models.Rss;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using BioEngine.Site.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using BioEngine.Content.Helpers;
using BioEngine.Data;
using BioEngine.Site.Filters;
using BioEngine.Site.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                    options.AddMetricsResourceFilter();
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddDistributedMemoryCache();
            services.AddResponseCaching();
            services.AddIpbOauthAuthentication(Configuration);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(3);
                options.Cookie.Name = ".BioWareRu.Session";
                options.Cookie.HttpOnly = true;
            });

            services.Configure<AppSettings>(Configuration.GetSection("Application"));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddBioEngineRouting();

            services.AddScoped<BannerProvider>();
            services.AddScoped<IContentHelperInterface, ContentHelper>();
            services.AddScoped<IPBApiHelper>();
            services.AddScoped<IChannelProvider, RssProvider>();

            services.Configure<IPBApiConfig>(o =>
            {
                o.ApiKey = Configuration["BE_IPB_API_KEY"];
                o.ApiUrl = Configuration["BE_IPB_API_URL"];
                o.NewsForumId = Configuration["BE_IPB_NEWS_FORUM_ID"];
                o.DevMode = false;
            });
            services.Configure<PatreonConfig>(o =>
            {
                o.ServiceUrl = new Uri(Configuration["BE_PATREON_SERVICE_URL"]);
            });
            services.Configure<AdminAccessConfig>(o => o.AdminAccessToken = Configuration["BE_ADMIN_ACCESS_TOKEN"]);

            services.AddSingleton<PatreonApiHelper>();

            if (_env.IsProduction())
            {
                var resolved = TryResolveDns(Configuration["BE_REDIS_HOST"]);
                var redisConfiguration = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        new IPEndPoint(resolved.AddressList.First(), int.Parse(Configuration["BE_REDIS_PORT"]))
                    },
                    AbortOnConnectFail = false,
                    DefaultDatabase = int.Parse(Configuration["BE_REDIS_DATABASE"])
                };

                var redis = ConnectionMultiplexer.Connect(redisConfiguration);
                services.AddDataProtection().PersistKeysToRedis(redis, "DataProtection-Keys");
                services.AddAntiforgery(opts => opts.Cookie.Name = "beAntiforgeryCookie");
            }

            var builder = services.AddBioEngineData(Configuration);

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private static IPHostEntry TryResolveDns(string redisUrl)
        {
            var ip = Dns.GetHostEntryAsync(redisUrl).GetAwaiter().GetResult();
            return ip;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("ru-RU"),
                new CultureInfo("ru")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseMiddleware<LoggingMiddleware>();

            if (env.IsProduction())
            {
                ConfigureProduction(app);
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
                routes.UseBioEngineRouting();
            });

            lifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

        private static void ConfigureProduction(IApplicationBuilder app)
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            };
            options.KnownProxies.Clear();
            options.KnownNetworks.Clear();
            options.RequireHeaderSymmetry = false;
            app.UseForwardedHeaders(options);

            app.UseExceptionHandler("/error/500");
        }
    }
}