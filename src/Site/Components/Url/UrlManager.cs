using System;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components.Url
{
    public class UrlManager
    {
        public readonly ArticleUrlManager Articles;
        public readonly FileUrlManager Files;
        public readonly GalleryUrlManager Gallery;
        public readonly GameUrlManager Games;
        public readonly DeveloperUrlManager Developer;
        public readonly TopicUrlManager Topics;
        public readonly NewsUrlManager News;

        private AppSettings Settings;

        public UrlManager(BWContext dbContext, IActionContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory, IOptions<AppSettings> options)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            Settings = options.Value;

            Articles = new ArticleUrlManager(Settings, dbContext, urlHelper);
            Files = new FileUrlManager(Settings, dbContext, urlHelper);
            Gallery = new GalleryUrlManager(Settings, dbContext, urlHelper);
            Games = new GameUrlManager(Settings, dbContext, urlHelper);
            Developer = new DeveloperUrlManager(Settings, dbContext, urlHelper);
            Topics = new TopicUrlManager(Settings, dbContext, urlHelper);
            News = new NewsUrlManager(Settings, dbContext, urlHelper);
        }

        public string ParentUrl(ParentModel parent)
        {
            switch (parent.Type)
            {
                case ParentType.Game:
                    return Games.PublicUrl((Game) parent);
                case ParentType.Developer:
                    return Developer.PublicUrl((Developer) parent);
                case ParentType.Topic:
                    return Topics.PublicUrl((Topic) parent);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
    }
}