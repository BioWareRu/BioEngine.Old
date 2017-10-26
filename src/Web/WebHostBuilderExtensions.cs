﻿using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using App.Metrics.Reporting.InfluxDB.Client;
using BioEngine.Web.Health;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Graylog;

namespace BioEngine.Web
{
    public static class WebHostBuilderExtensions
    {
        private static IWebHostBuilder ConfigureSerilog(this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext> configureSerilog)
        {
            return hostBuilder.ConfigureServices((context, services) => { configureSerilog(context); });
        }

        public static IWebHostBuilder AddAppMetrics(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((builder, services) =>
                {
                    if (builder.HostingEnvironment.IsProduction())
                    {
                        services.Configure<GraylogHealthCheckOptions>(o =>
                        {
                            o.Uri = new Uri(builder.Configuration["BE_GRAYLOG_STATUS_URL"]);
                        });
                    }
                })
                .ConfigureMetricsWithDefaults((whBuilder, metricsBuilder) =>
                {
                    metricsBuilder.Configuration.Configure(options =>
                    {
                        options.DefaultContextLabel = whBuilder.Configuration["BW_APP_NAME"];
                        options.GlobalTags["env"] = whBuilder.HostingEnvironment.IsStaging() ? "stage" :
                            whBuilder.HostingEnvironment.IsProduction() ? "prod" : "dev";
                    });
                    if (whBuilder.HostingEnvironment.IsDevelopment())
                    {
                        //metricsBuilder.Report.ToConsole(TimeSpan.FromMinutes(5)); // uncomment if needed
                    }
                    else
                    {
                        metricsBuilder.Report.ToInfluxDb(options =>
                        {
                            options.HttpPolicy = new HttpPolicy
                            {
                                FailuresBeforeBackoff = 3,
                                BackoffPeriod = TimeSpan.FromSeconds(30),
                                Timeout = TimeSpan.FromSeconds(3)
                            };
                            options.InfluxDb.Database = whBuilder.Configuration["BE_INFLUXDB_DB"];
                            options.InfluxDb.BaseUri = new Uri(whBuilder.Configuration["BE_INFLUXDB_HOST"]);
                            options.FlushInterval = TimeSpan.FromSeconds(1);
                        });
                    }
                })
                .UseMetrics<DefaultMetricsStartupFilter>()
                .ConfigureHealthWithDefaults(
                    builder =>
                    {
                        builder.HealthChecks.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    })
                .UseHealth();

            return hostBuilder;
        }

        public static IWebHostBuilder AddSerilog(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureSerilog(hostingContext =>
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
                .UseSerilog();
            return hostBuilder;
        }
    }
}