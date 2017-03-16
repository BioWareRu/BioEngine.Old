using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Search;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.Helpers;
using cloudscribe.Syndication.Models.Rss;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
                    options.Filters.Add(typeof(ExceptionFilter));
                    options.Filters.Add(typeof(CounterFilter));
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddTypedRouting();
            services.AddAuthentication(options => options.SignInScheme =
                CookieAuthenticationDefaults.AuthenticationScheme);
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddSingleton<DBConfiguration, MySqlDBConfiguration>();

            services.AddDbContext<BWContext>();
            services.AddScoped<BannerProvider>();
            services.AddScoped<UrlManager>();
            services.AddScoped<ParentEntityProvider>();
            services.AddScoped<ContentHelper>();
            services.AddScoped<IChannelProvider, RssProvider>();
            services.AddScoped(typeof(ISearchProvider<>), typeof(ElasticSearchProvider<>));

            services.AddDistributedMemoryCache();

            services.AddResponseCaching();

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
            }

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".BioWareRu.Session";
                options.CookieHttpOnly = true;
            });
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
                app.UseExceptionHandler("/Home/Error");
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

            if (env.IsProduction())
            {
                var options = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                };
                options.KnownProxies.Clear();
                options.KnownNetworks.Clear();
                options.RequireHeaderSymmetry = false;
                app.UseForwardedHeaders(options);
            }

            app.UseStaticFiles();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseMiddleware<CounterMiddleware>();

            app.UseStatusCodePages();

            app.UseResponseCaching();

            app.UseSession();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/login")
            });

            app.UseOAuthAuthentication(new OAuthOptions()
            {
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                AuthenticationScheme = "IPB",
                DisplayName = "IPB",
                ClientId = Configuration["IPB_OAUTH_CLIENT_ID"],
                ClientSecret = Configuration["IPB_OAUTH_CLIENT_SECRET"],
                CallbackPath = new PathString(Configuration["Data:OAuth:CallbackPath"]),
                AuthorizationEndpoint = Configuration["Data:OAuth:AuthorizationEndpoint"],
                TokenEndpoint = Configuration["Data:OAuth:TokenEndpoint"],
                UserInformationEndpoint = Configuration["Data:OAuth:UserInformationEndpoint"],
                SaveTokens = true,
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        // Get the GitHub user
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        var identifier = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(identifier))
                        {
                            context.Identity.AddClaim(new Claim(
                                ClaimTypes.NameIdentifier, identifier,
                                ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        var userName = user.Value<string>("displayName");
                        if (!string.IsNullOrEmpty(userName))
                        {
                            context.Identity.AddClaim(new Claim(
                                ClaimsIdentity.DefaultNameClaimType, userName,
                                ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        var profileUrl = user.Value<string>("profileUrl");
                        if (!string.IsNullOrEmpty(profileUrl))
                        {
                            context.Identity.AddClaim(new Claim(
                                ClaimTypes.Webpage, profileUrl,
                                ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        var link = user.Value<string>("avatar");
                        if (!string.IsNullOrEmpty(link))
                        {
                            context.Identity.AddClaim(new Claim(
                                "avatarUrl", link,
                                ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }
                    }
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
            });

            if (env.IsProduction())
            {
                lifetime.ApplicationStarted.Register(RunPrometheus);
                lifetime.ApplicationStopped.Register(StopPrometheus);
            }
            lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
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