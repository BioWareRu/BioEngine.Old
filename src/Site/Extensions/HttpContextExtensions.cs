using Microsoft.AspNetCore.Http;

namespace BioEngine.Site.Extensions
{
    public static class HttpContextExtensions
    {
        public static string AbsoluteUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }
    }
}