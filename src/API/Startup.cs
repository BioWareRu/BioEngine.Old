using System.IO;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST.Validators;
using BioEngine.Common.DB;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Graylog;

namespace BioEngine.API
{
    public class Startup
    {
        public static readonly LoggingLevelSwitch LogLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configs" + Path.DirectorySeparatorChar + "appsettings.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenAuthOptions>(o =>
            {
                o.AutomaticChallenge = true;
                o.AuthenticationScheme = "tokenAuth";
                o.ClientId = Configuration["IPB_OAUTH_CLIENT_ID"];
            });

            services.AddSingleton(Configuration);
            services.AddSingleton<DBConfiguration, MySqlDBConfiguration>();

            services.AddDbContext<BWContext>();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLogging(env, loggerFactory);
            app.UseMiddleware<TokenAuthMiddleware>();
            app.UseCors("allorigins");
            app.UseMvc();
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