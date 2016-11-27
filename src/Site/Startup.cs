using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BioEngine.Site.Components;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Site
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
               .AddJsonFile("Configs" + Path.DirectorySeparatorChar + "appsettings.json")
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + $"config.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            .AddDataAnnotationsLocalization();

            services.AddSingleton<IAuthorizationHandler, IpbAuthorizationHandler>();
            services.Configure<DBConfiguration>(Configuration.GetSection("Data:Mysql"));
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.Configure<IpbAuthenticationOptions>(o => { o.AutomaticChallenge = true; o.AuthenticationScheme = "ipb"; });
            services.AddDbContext<BWContext>();
            services.AddScoped<BannerManager>();
            services.AddSingleton<UrlManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        new CultureInfo("ru"),
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

            app.UseMiddleware<IPBAuthenticationMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Index}/{action=Index}/{id?}");
            });
        }
    }
}
