using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.News
{
    internal static class NewsRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(NewsRoutesEnum.Index, "index.html", new {controller = "News", action = "Index"});
            routeBuilder.MapRoute(NewsRoutesEnum.IndexWithPage, "page/{p:int}.html",
                new {controller = "News", action = "Index"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByYear, "{year:int}.html",
                new {controller = "News", action = "NewsByYear"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByYearWithPage, "{year:int}/page/{p:int}.html",
                new {controller = "News", action = "NewsByYear"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByMonth, "{year:int}/{month:regex(\\d{{2}})}.html",
                new {controller = "News", action = "NewsByYearAndMonth"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByMonthWithPage,
                "{year:int}/{month:regex(\\d{{2}})}/page/{p:int}.html",
                new {controller = "News", action = "NewsByYearAndMonth"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByDay,
                "{year:int}/{month:regex(\\d{{2}})}/{day:regex(\\d{{2}})}.html",
                new {controller = "News", action = "NewsByYearAndMonthAndDay"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByDayWithPage,
                "{year:int}/{month:regex(\\d{{2}})}/{day:regex(\\d{{2}})}/page/{p:int}.html",
                new {controller = "News", action = "NewsByYearAndMonthAndDay"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByParent, "{parentUrl}/news.html",
                new {controller = "News", action = "ParentNews"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsByParentWithPage, "{parentUrl}/news/page/{page}.html",
                new {controller = "News", action = "ParentNewsWithPage"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsPage,
                "{year:int}/{month:regex(^\\d{{2}}$)}/{day:regex(^\\d{{2}}$)}/{url}.html",
                new {controller = "News", action = "Show"});
            routeBuilder.MapRoute(NewsRoutesEnum.Rss, "rss.xml",
                new {controller = "News", action = "Rss"});
            routeBuilder.MapRoute(NewsRoutesEnum.RssXml, "rss",
                new {controller = "News", action = "Rss"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsRss, "news/rss.xml",
                new {controller = "News", action = "Rss"});
            routeBuilder.MapRoute(NewsRoutesEnum.NewsRssXml, "news/rss",
                new {controller = "News", action = "Rss"});
        }
    }
}