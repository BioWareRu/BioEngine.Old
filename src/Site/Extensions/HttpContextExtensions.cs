using System;
using Microsoft.AspNetCore.Http;

namespace BioEngine.Site.Extensions
{
    public static class HttpContextExtensions
    {
        public static Uri AbsoluteUrl(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
        }
    }
}