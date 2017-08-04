using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.Search
{
    internal static class SearchRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(SearchRoutesEnum.BlockPage, "search.html",
                new {controller = "Search", action = "Index"});
        }
    }
}