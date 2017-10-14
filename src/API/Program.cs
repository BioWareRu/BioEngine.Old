using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.Graylog;

namespace BioEngine.API
{
    [UsedImplicitly]
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureSerilog(hostingContext =>
                {
                    var loggerConfiguration =
                        new LoggerConfiguration().Enrich.FromLogContext();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration = loggerConfiguration
                            .WriteTo.LiterateConsole().MinimumLevel.Debug();
                    }
                    else
                    {
                        loggerConfiguration = loggerConfiguration
                            .WriteTo.Graylog(new GraylogSinkOptions
                            {
                                HostnameOrAdress = hostingContext.Configuration["BE_GELF_HOST"],
                                Port = int.Parse(hostingContext.Configuration["BE_GELF_PORT"]),
                                Facility = hostingContext.Configuration["BE_GELF_FACILITY"]
                            }).MinimumLevel.Error();
                    }

                    Log.Logger = loggerConfiguration.CreateLogger();
                })
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();
    }

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureSerilog(this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext> configureSerilog)
        {
            return hostBuilder.ConfigureServices((context, services) => { configureSerilog(context); });
        }
    }
}