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

        public Uri PublicUrl(Game game, bool absolute = false)
        {
            return GetUrl(BaseRoutesEnum.GamePage, new {gameUrl = game.Url}, absolute);
        }

        public Uri PublicUrl(Developer developer, bool absolute = false)
        {
            return UrlHelper.News().ParentNewsUrl(developer, absolute: absolute);
        }

        public Uri PublicUrl(Topic topic, bool absolute = false)
        {
            return null;
        }

        public Uri ParentUrl(IParentModel parent, bool absoluteUrl = false)
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

        public Uri ParentIconUrl(IChildModel child)
        {
            return ParentIconUrl((dynamic) child.Parent);
        }

        public Uri ParentIconUrl(IParentModel parent)
        {
            return ParentIconUrl((dynamic) parent);
        }

        public Uri ParentIconUrl(Developer developer)
        {
            return new Uri(Settings.AssetsDomain + Settings.DevelopersImagesPath + developer.Icon);
        }

        public Uri ParentIconUrl(Game game)
        {
            return new Uri(Settings.AssetsDomain + Settings.GamesImagesPath + "small/" + game.Icon);
        }

        public Uri ParentIconUrl(Topic topic)
        {
            return new Uri(Settings.AssetsDomain + Settings.TopicsImagesPath + topic.Icon);
        }

        public Uri GameLogoUrl(Game game)
        {
            return new Uri(Settings.AssetsDomain + Settings.GamesImagesPath + "big/" + game.Logo);
        }
    }
}