using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components.REST;
using BioEngine.API.Models;
using BioEngine.Common.Base;
using BioEngine.Data.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Ipb;
using BioEngine.Content.Helpers;
using BioEngine.Routing;
using BioEngine.Data;
using BioEngine.Social;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BioEngine.API
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

        // This method gets called by the runtime. Use this method to add services to the container
        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            bool.TryParse(Configuration["IPB_OAUTH_DEV_MODE"] ?? "", out var devMode);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "token";
                    options.DefaultChallengeScheme = "token";
                })
                .AddScheme<TokenAuthOptions, TokenAuthenticationHandler>("token",
                    options =>
                    {
                        options.ClientId = Configuration["IPB_OAUTH_CLIENT_ID"];
                        options.UserInformationEndpointUrl = Configuration["Data:OAuth:UserInformationEndpoint"];
                        options.DevMode = devMode;
                    });


            services.AddDistributedMemoryCache();
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddBioEngineRouting();
            services.AddScoped<IContentHelperInterface, ContentHelper>();
            services.AddScoped<IPBApiHelper>();

            services.Configure<IPBApiConfig>(o =>
            {
                o.ApiKey = Configuration["BE_IPB_API_KEY"];
                o.ApiUrl = Configuration["BE_IPB_API_URL"];
                o.NewsForumId = Configuration["BE_IPB_NEWS_FORUM_ID"];
                o.DevMode = devMode;
            });

            services.Configure<APISettings>(o => o.FileBrowserUrl = Configuration["API_FILE_BROWSER_URL"]);

            services.AddBioEngineSocial(Configuration);

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

            services.AddCors(options =>
            {
                // Define one or more CORS policies
                options.AddPolicy("allorigins",
                    corsBuilder =>
                    {
                        corsBuilder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowCredentials();
                    });
            });
            services.AddMvc(options => { options.AddMetricsResourceFilter(); });


            var builder = services.AddBioEngineData(Configuration);

            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("MapperProfile")).As<Profile>();

            builder.RegisterGeneric(typeof(RestContext<>)).InstancePerDependency();

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private static IPHostEntry TryResolveDns(string redisUrl)
        {
            var ip = Dns.GetHostEntryAsync(redisUrl).GetAwaiter().GetResult();
            return ip;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, BWContext context)
        {
            if (_env.IsProduction() && context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            app.UseAuthentication();
            app.UseCors("allorigins");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
                routes.UseBioEngineRouting();
            });
            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}