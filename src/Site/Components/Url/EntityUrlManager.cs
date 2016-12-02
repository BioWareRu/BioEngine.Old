using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
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
            return url;
        }

        public string ParentIconUrl(Developer developer)
        {
            return Settings.AssetsDomain
                   + Settings.DevelopersImagesPath + developer.Icon;
        }

        public string ParentIconUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "small/" + game.Icon;
        }

        public string ParentIconUrl(Topic topic)
        {
            return Settings.AssetsDomain + Settings.TopicsImagesPath + topic.Icon;
        }

        protected string ParentUrl(ChildModel child)
        {
            return child.Parent.ParentUrl;
        }
    }
}