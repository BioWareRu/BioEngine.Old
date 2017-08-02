using BioEngine.Routing.Articles;
using BioEngine.Routing.Base;
using BioEngine.Routing.Files;
using BioEngine.Routing.Gallery;
using BioEngine.Routing.News;
using BioEngine.Routing.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Routing
{
    public static class ServicesCollectionExtensions
    {
        public static void AddBioEngineRouting(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<BaseUrlManager>();
            services.AddScoped<NewsUrlManager>();
            services.AddScoped<ArticlesUrlManager>();
            services.AddScoped<FilesUrlManager>();
            services.AddScoped<GalleryUrlManager>();
            services.AddScoped<SearchUrlManager>();
        }
    }
}