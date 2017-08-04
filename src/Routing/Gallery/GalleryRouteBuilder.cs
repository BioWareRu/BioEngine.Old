using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.Gallery
{
    internal static class GalleryRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(GalleryRoutesEnum.CatPage, "{parentUrl}/gallery/{*url}",
                new {controller = "Gallery", action = "Cat"});
            routeBuilder.MapRoute(GalleryRoutesEnum.ParentPage, "{parentUrl}/gallery.html",
                new {controller = "Gallery", action = "ParentGallery"});
        }
    }
}
