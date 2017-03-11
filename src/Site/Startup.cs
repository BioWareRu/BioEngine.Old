using System;
using System.Globalization;
using System.IO;
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
            services.AddMvc()
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

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".BioWareRu.Session";
                options.CookieHttpOnly = true;
            });
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseStatusCodePages();


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
        }
    }
}