using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components
{
    public abstract class EntityUrlManager
    {
        protected readonly BWContext _dbContext;
        protected readonly AppSettings _settings;
        protected readonly IUrlHelper _urlHelper;

        protected EntityUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
        {
            _settings = settings;
            _dbContext = dbContext;
            _urlHelper = urlHelper;
        }

        protected string GetUrl(string action, string controller, object urlParams, bool absolute = false)
        {
            var url = _urlHelper.Action(action, controller, urlParams,
                absolute ? _urlHelper.ActionContext.HttpContext.Request.Scheme : null);
            url = url.Replace("%2F", "/"); //TODO: Ugly hack because of https://github.com/aspnet/Routing/issues/363
            return url;
        }

        public string ParentIconUrl(Developer developer)
        {
            return _settings.AssetsDomain
                   + _settings.DevelopersImagesPath + developer.Icon;
        }

        public string ParentIconUrl(Game game)
        {
            return _settings.AssetsDomain + _settings.GamesImagesPath + "small/" + game.Icon;
        }

        public string ParentIconUrl(Topic topic)
        {
            return _settings.AssetsDomain + _settings.TopicsImagesPath + topic.Icon;
        }

        protected string ParentUrl(ChildModel child)
        {
            return child.Parent.ParentUrl;
        }
    }
}