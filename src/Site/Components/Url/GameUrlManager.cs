using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class GameUrlManager : EntityUrlManager
    {
        public GameUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
        {
        }

        public string PublicUrl(Game game, bool absolute = false)
        {
            return GetUrl("Index", "Games", new {gameUrl = game.Url}, absolute);
        }

        public string LogoUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "big/" + game.Logo;
        }
    }
}