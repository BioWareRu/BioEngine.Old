using System;
using BioEngine.Routing.Articles;
using BioEngine.Routing.Base;
using BioEngine.Routing.Files;
using BioEngine.Routing.Gallery;
using BioEngine.Routing.News;
using BioEngine.Routing.Search;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace BioEngine.Routing
{
    public static class RouteBuilderExtensions
    {
        public static void UseBioEngineRouting(this IRouteBuilder routeBuilder)
        {
            BaseRouteBuilder.Register(routeBuilder);
            NewsRouteBuilder.Register(routeBuilder);
            ArticlesRouteBuilder.Register(routeBuilder);
            FilesRouteBuilder.Register(routeBuilder);
            GalleryRouteBuilder.Register(routeBuilder);
            SearchRouteBuilder.Register(routeBuilder);
        }

        internal static void MapRoute(this IRouteBuilder routeBuilder, Enum routeEnum, string template, object defaults)
        {
            var routeName = Enum.GetName(routeEnum.GetType(), routeEnum);
            routeBuilder.MapRoute(routeName, template, defaults);
        }
    }
}