using System;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
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
        public readonly SearchUrlManager Search;

        private AppSettings Settings;
        private ParentEntityProvider _parentEntityProvider;

        public UrlManager(BWContext dbContext, IActionContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory, IOptions<AppSettings> options,
            ParentEntityProvider parentEntityProvider)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            Settings = options.Value;
            _parentEntityProvider = parentEntityProvider;
            Articles = new ArticleUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Files = new FileUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Gallery = new GalleryUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Games = new GameUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Developer = new DeveloperUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Topics = new TopicUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            News = new NewsUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
            Search = new SearchUrlManager(Settings, dbContext, urlHelper, parentEntityProvider);
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

        public async Task<string> ParentIconUrl(IChildModel child)
        {
            var parent = await _parentEntityProvider.GetModelParent(child);
            return ParentIconUrl((dynamic) parent);
        }

        public async Task<string> ParentIconUrl(Developer developer)
        {
            return await Task.FromResult(Settings.AssetsDomain
                   + Settings.DevelopersImagesPath + developer.Icon);
        }

        public async Task<string> ParentIconUrl(Game game)
        {
            return await Task.FromResult(Settings.AssetsDomain + Settings.GamesImagesPath + "small/" + game.Icon);
        }

        public async Task<string> ParentIconUrl(Topic topic)
        {
            return await Task.FromResult(Settings.AssetsDomain + Settings.TopicsImagesPath + topic.Icon);
        }
    }
}