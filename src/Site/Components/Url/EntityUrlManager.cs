using System;
using System.Threading.Tasks;
using BioEngine.Common.Base;
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
        protected readonly ParentEntityProvider ParentEntityProvider;

        protected EntityUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
        {
            Settings = settings;
            DbContext = dbContext;
            UrlHelper = urlHelper;
            ParentEntityProvider = parentEntityProvider;
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


        protected async Task<string> ParentUrl(IChildModel child)
        {
            return (await ParentEntityProvider.GetModelParent(child)).ParentUrl;
        }
    }
}