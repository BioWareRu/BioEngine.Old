using System;
using System.IO;
using System.Text.RegularExpressions;
using ImageSharp;
using ImageSharp.Processing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BioEngine.Resizr
{
    public class Startup
    {
        private static readonly Regex PathRegex = new Regex(
            "(?<folderPath>.*)/(?<imageName>.*?).(?<width>[0-9]+)x(?<height>[0-9]+).(?<format>[a-zA-Z]+)");

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            var logger = loggerFactory.CreateLogger("Resizr");
            logger.LogInformation($"Root: {Configuration["ROOT_PATH"]}");

            app.Run(async context =>
            {
                var path = System.Net.WebUtility.UrlDecode(context.Request.Path);
                var match = PathRegex.Match(path);
                if (match.Success)
                {
                    var folderPath = match.Groups["folderPath"].Value;
                    var imageName = match.Groups["imageName"].Value;
                    var format = match.Groups["format"].Value;

                    if (!int.TryParse(match.Groups["width"].Value, out int width))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Bad width");
                        return;
                    }
                    if (!int.TryParse(match.Groups["height"].Value, out int height))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Bad height");
                        return;
                    }
                    var destPath = $"{Configuration["ROOT_PATH"]}/{folderPath}/{imageName}.{width}x{height}.{format}";
                    if (File.Exists(destPath))
                    {
                        await context.Response.SendFileAsync(destPath);
                    }
                    else
                    {
                        var sourcePath = $"{Configuration["ROOT_PATH"]}/{folderPath}/{imageName}.{format}";
                        if (File.Exists(sourcePath))
                        {
                            var resizeOptions = new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                Size = new Size(width, height)
                            };
                            try
                            {
                                using (var image = Image.Load(sourcePath))
                                {
                                    image.Resize(resizeOptions)
                                        .Save(destPath);
                                    context.Response.ContentType = image.CurrentImageFormat.MimeType;
                                }
                                await context.Response.SendFileAsync(destPath);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex.Message);
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                await context.Response.WriteAsync("Internal error");
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            await context.Response.WriteAsync("File not found");
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Bad Url");
                }
            });
        }
    }
}