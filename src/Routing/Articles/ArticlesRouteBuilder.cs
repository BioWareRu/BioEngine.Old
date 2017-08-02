using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.Articles
{
    internal static class ArticlesRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(ArticlesRoutesEnum.ArticlePage, "{parentUrl}/articles/{*url}",
                new {controller = "Articles", action = "Show"});
            routeBuilder.MapRoute(ArticlesRoutesEnum.ArticlePageOld, "articles/{parentUrl}/{*url}",
                new {controller = "Articles", action = "Show"});
            routeBuilder.MapRoute(ArticlesRoutesEnum.ArticlesByParent, "{parentUrl}/articles.html",
                new {controller = "Articles", action = "ParentArticles"});
        }
    }
}