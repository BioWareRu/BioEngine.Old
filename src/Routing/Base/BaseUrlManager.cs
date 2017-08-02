using System;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Base
{
    public class BaseUrlManager : UrlManager<BaseRoutesEnum>
    {
        public BaseUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper, appSettings)
        {
        }

        public string PublicUrl(Game game, bool absolute = false)
        {
            return GetUrl(BaseRoutesEnum.GamePage, new {gameUrl = game.Url}, absolute);
        }

        public string PublicUrl(Developer developer, bool absolute = false)
        {
            return UrlHelper.News().ParentNewsUrl(developer, absolute: absolute);
        }

        public string PublicUrl(Topic topic, bool absolute = false)
        {
            return "#";
        }

        public string ParentUrl(IParentModel parent, bool absoluteUrl = false)
        {
            switch (parent.Type)
            {
                case ParentType.Game:
                    return PublicUrl((Game) parent, absoluteUrl);
                case ParentType.Developer:
                    return PublicUrl((Developer) parent, absoluteUrl);
                case ParentType.Topic:
                    return PublicUrl((Topic) parent, absoluteUrl);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string ParentIconUrl(IChildModel child)
        {
            return ParentIconUrl((dynamic) child.Parent);
        }

        public string ParentIconUrl(IParentModel parent)
        {
            return ParentIconUrl((dynamic) parent);
        }

        public string ParentIconUrl(Developer developer)
        {
            return Settings.AssetsDomain + Settings.DevelopersImagesPath + developer.Icon;
        }

        public string ParentIconUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "small/" + game.Icon;
        }

        public string ParentIconUrl(Topic topic)
        {
            return Settings.AssetsDomain + Settings.TopicsImagesPath + topic.Icon;
        }

        public string GameLogoUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "big/" + game.Logo;
        }
    }
}