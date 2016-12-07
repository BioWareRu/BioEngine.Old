using System;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public abstract class EntityUrlManager
    {
        protected readonly BWContext DbContext;
        protected readonly AppSettings Settings;
        protected readonly IUrlHelper UrlHelper;

        protected EntityUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
        {
            Settings = settings;
            DbContext = dbContext;
            UrlHelper = urlHelper;
        }

        protected string GetUrl(string action, string controller, object urlParams, bool absolute = false)
        {
            var url = UrlHelper.Action(action, controller, urlParams,
                absolute ? UrlHelper.ActionContext.HttpContext.Request.Scheme : null);
            url = url.Replace("%2F", "/"); //TODO: Ugly hack because of https://github.com/aspnet/Routing/issues/363
            if (url.IndexOf(".html", StringComparison.Ordinal) < 0)
            {
                url += ".html";
            }
            return url;
        }


        protected string ParentUrl(IChildModel child)
        {
            return child.Parent.ParentUrl;
        }
    }
}