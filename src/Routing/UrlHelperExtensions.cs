using BioEngine.Routing.Articles;
using BioEngine.Routing.Base;
using BioEngine.Routing.Core;
using BioEngine.Routing.Files;
using BioEngine.Routing.Gallery;
using BioEngine.Routing.News;
using BioEngine.Routing.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Routing
{
    public static class UrlHelperExtensions
    {
        public static BaseUrlManager Base(this IUrlHelper urlHelper) => GetUrlManager<BaseUrlManager>(urlHelper);
        public static NewsUrlManager News(this IUrlHelper urlHelper) => GetUrlManager<NewsUrlManager>(urlHelper);

        public static ArticlesUrlManager Articles(this IUrlHelper urlHelper) =>
            GetUrlManager<ArticlesUrlManager>(urlHelper);

        public static FilesUrlManager Files(this IUrlHelper urlHelper) => GetUrlManager<FilesUrlManager>(urlHelper);

        public static GalleryUrlManager Gallery(this IUrlHelper urlHelper) =>
            GetUrlManager<GalleryUrlManager>(urlHelper);

        public static SearchUrlManager Search(this IUrlHelper urlHelper) => GetUrlManager<SearchUrlManager>(urlHelper);


        private static T GetUrlManager<T>(IUrlHelper urlHelper) where T : UrlManager
        {
            return urlHelper.ActionContext.HttpContext.RequestServices.GetService<T>();
        }
    }
}