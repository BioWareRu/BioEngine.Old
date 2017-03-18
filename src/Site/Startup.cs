using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Ipb;
using BioEngine.Common.Search;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.Helpers;
using cloudscribe.Syndication.Models.Rss;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog.Core;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using BioEngine.Site.Middlewares;
using BioEngine.Site.Filters;
using BioEngine.Prometheus.Core;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace BioEngine.Site
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + "appsettings.json")
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + $"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(options =>
                {
                    //options.Filters.Add(typeof(ExceptionFilter));
                    options.Filters.Add(typeof(CounterFilter));
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddTypedRouting();
            services.AddDistributedMemoryCache();
            services.AddResponseCaching();
            services.AddAuthentication(options => options.SignInScheme =
                CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".BioWareRu.Session";
                options.CookieHttpOnly = true;
            });

            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddDbContext<BWContext>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddSingleton<DBConfiguration, MySqlDBConfiguration>();

            services.AddScoped<BannerProvider>();
            services.AddScoped<UrlManager>();
            services.AddScoped<ParentEntityProvider>();
            services.AddScoped<ContentHelper>();
            services.AddScoped<IPBApiHelper>();
            services.AddScoped<IChannelProvider, RssProvider>();
            services.AddScoped(typeof(ISearchProvider<>), typeof(ElasticSearchProvider<>));

            if (_env.IsProduction())
            {
                var resolved = TryResolveDns(Configuration["BE_REDIS_HOST"]);
                var redisConfiguration = new ConfigurationOptions()
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
                services.AddAntiforgery(opts => opts.CookieName = "beAntiforgeryCookie");
            }
        }

        private static IPHostEntry TryResolveDns(string redisUrl)
        {
            var ip = Dns.GetHostEntryAsync(redisUrl).GetAwaiter().GetResult();
            return ip;
        }

        public static readonly LoggingLevelSwitch LogLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime)
        {
            ConfigureLogging(env, loggerFactory);

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

            app.UseMiddleware<CounterMiddleware>();

            if (env.IsProduction())
            {
                ConfigureProduction(app, lifetime);
            }

            app.UseResponseCaching();

            app.UseSession();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/login"),
                ExpireTimeSpan = TimeSpan.FromDays(30)
            });

            var ipbLogger = loggerFactory.CreateLogger("IpbAuthLogger");

            app.UseIpbOAuthAuthentication(Configuration, ipbLogger);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
            });
        }

        private void ConfigureProduction(IApplicationBuilder app, IApplicationLifetime lifetime)
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

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            lifetime.ApplicationStarted.Register(RunPrometheus);
            lifetime.ApplicationStopped.Register(StopPrometheus);
            lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private void ConfigureLogging(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggerConfiguration =
                new LoggerConfiguration().MinimumLevel.ControlledBy(LogLevelSwitch).Enrich.FromLogContext();

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                loggerConfiguration = loggerConfiguration
                    .WriteTo.LiterateConsole();
                LogLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            }
            else
            {
                loggerConfiguration = loggerConfiguration
                    .WriteTo.Graylog(new GraylogSinkOptions()
                    {
                        HostnameOrAdress = Configuration["BE_GELF_HOST"],
                        Port = int.Parse(Configuration["BE_GELF_PORT"]),
                        Facility = Configuration["BE_GELF_FACILITY"]
                    });
            }
            Log.Logger = loggerConfiguration.CreateLogger();
            loggerFactory.AddSerilog();
        }

        private MetricServer _metricServer;

        private void RunPrometheus()
        {
            _metricServer = new MetricServer("*", int.Parse(Configuration["BE_PROMETHEUS_PORT"]));
            _metricServer.Start();
        }

        private void StopPrometheus()
        {
            _metricServer.Stop();
        }
    }
}