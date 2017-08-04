using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.Base
{
    internal static class BaseRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(BaseRoutesEnum.GamePage, "{gameUrl:regex(^[a-z0-9_]+$)}.html",
                new {controller = "Games", action = "Index"});
        }
    }
}