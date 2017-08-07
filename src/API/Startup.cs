using System;
using System.IO;
using System.Linq;
using System.Net;
using BioEngine.API.Auth;
using BioEngine.API.Components.REST.Validators;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Ipb;
using BioEngine.Content.Helpers;
using BioEngine.Routing;
using BioEngine.Data;
using FluentValidation.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using StackExchange.Redis;

namespace BioEngine.API
{
    [UsedImplicitly]
    public class Startup
    {
        private static readonly LoggingLevelSwitch LogLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + "appsettings.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenAuthOptions>(o =>
            {
                o.AutomaticChallenge = true;
                o.AuthenticationScheme = "tokenAuth";
                o.ClientId = Configuration["IPB_OAUTH_CLIENT_ID"];
            });

            services.AddDistributedMemoryCache();
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddBioEngineRouting();
            services.AddBioEngineData(Configuration);
            services.AddScoped<IContentHelperInterface, ContentHelper>();
            services.AddScoped<IPBApiHelper>();

            services.AddSingleton(new IPBApiConfig
            {
                ApiKey = Configuration["BE_IPB_API_KEY"],
                ApiUrl = Configuration["BE_IPB_API_URL"],
                NewsForumId = Configuration["BE_IPB_NEWS_FORUM_ID"]
            });

            services.AddSingleton(new PatreonConfig(new Uri(Configuration["BE_PATREON_API_URL"]),
                Configuration["BE_PATREON_API_KEY"]));
            services.AddSingleton<PatreonApiHelper>();

            services.AddSingleton(Configuration);
            services.AddSingleton<DBConfiguration, MySqlDBConfiguration>();

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
                services.AddAntiforgery(opts => opts.CookieName = "beAntiforgeryCookie");
            }

            services.AddCors(options =>
            {
                // Define one or more CORS policies
                options.AddPolicy("allorigins",
                    corsBuilder =>
                    {
                        corsBuilder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowCredentials();
                    });
            });

            // Add framework services.
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewsValidator>());
        }

        private static IPHostEntry TryResolveDns(string redisUrl)
        {
            var ip = Dns.GetHostEntryAsync(redisUrl).GetAwaiter().GetResult();
            return ip;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLogging(env, loggerFactory);
            app.UseMiddleware<TokenAuthMiddleware>();
            app.UseCors("allorigins");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
                routes.UseBioEngineRouting();
            });
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
                    .WriteTo.Graylog(new GraylogSinkOptions
                    {
                        HostnameOrAdress = Configuration["BE_GELF_HOST"],
                        Port = int.Parse(Configuration["BE_GELF_PORT"]),
                        Facility = Configuration["BE_GELF_FACILITY"]
                    });
            }
            Log.Logger = loggerConfiguration.CreateLogger();
            loggerFactory.AddSerilog();
        }
    }
}