using System.Globalization;
using System.IO;
using BioEngine.Common.DB;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Ipb;
using BioEngine.Site.Components.Url;
using cloudscribe.Syndication.Models.Rss;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace BioEngine.Site
{
    [UsedImplicitly]
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + "appsettings.json")
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + $"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        //.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("ipb")
                        .AddRequirements(new IpbRequestPassed())
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization().AddTypedRouting();

            services.AddSingleton<IAuthorizationHandler, IpbAuthorizationHandler>();
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.Configure<IpbAuthenticationOptions>(o =>
            {
                o.AutomaticChallenge = true;
                o.AuthenticationScheme = "ipb";
            });

            var mysqlConnBuilder = new MySqlConnectionStringBuilder
            {
                Server = Configuration["Data:Mysql:Host"],
                Port = uint.Parse(Configuration["Data:Mysql:Port"]),
                UserID = Configuration["Data:Mysql:Username"],
                Password = Configuration["Data:Mysql:Password"],
                Database = Configuration["Data:Mysql:Database"]
            };

            services.AddDbContext<BWContext>(builder => builder.UseMySql(mysqlConnBuilder.ConnectionString));
            services.AddScoped<BannerProvider>();
            services.AddScoped<UrlManager>();
            services.AddScoped<ParentEntityProvider>();
            services.AddScoped<IChannelProvider, RssProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


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

            app.UseStatusCodePages();

            app.UseMiddleware<IpbAuthenticationMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Index}/{action=Index}/{id?}");
            });
        }
    }
}