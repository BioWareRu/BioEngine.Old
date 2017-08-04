using Microsoft.AspNetCore.Routing;

namespace BioEngine.Routing.Files
{
    internal static class FilesRouteBuilder
    {
        public static void Register(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(FilesRoutesEnum.FilePage, "{parentUrl}/files/{*url}",
                new {controller = "Files", action = "Show"});
            routeBuilder.MapRoute(FilesRoutesEnum.FileDownloadPage, "{parentUrl}/download/{*url}",
                new {controller = "Files", action = "Download"});
            routeBuilder.MapRoute(FilesRoutesEnum.FilesByParent, "{parentUrl}/files.html",
                new {controller = "Files", action = "ParentFiles"});
        }
    }
}
