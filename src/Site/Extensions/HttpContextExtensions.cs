using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
