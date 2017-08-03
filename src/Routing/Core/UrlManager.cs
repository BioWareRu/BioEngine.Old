using System;
using BioEngine.Common.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Core
{
    public abstract class UrlManager
    {
    }

    public abstract class UrlManager<T> : UrlManager
    {
        protected readonly AppSettings Settings;
        protected readonly IUrlHelper UrlHelper;

        protected UrlManager(IUrlHelper urlHelper, IOptions<AppSettings> settings)
        {
            Settings = settings.Value;
            UrlHelper = urlHelper;
        }

        protected Uri GetUrl(T route, object urlParams = null, bool absolute = false)
        {
            var url = UrlHelper.RouteUrl(Enum.GetName(typeof(T), route), urlParams,
                absolute ? UrlHelper.ActionContext.HttpContext.Request.Scheme : null);
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("%2F", "/"); //TODO: Ugly hack because of https://github.com/aspnet/Routing/issues/363
                if (url.IndexOf(".html", StringComparison.Ordinal) < 0)
                    url += ".html";
                return new Uri(url);
            }
            return null;
        }
    }
}