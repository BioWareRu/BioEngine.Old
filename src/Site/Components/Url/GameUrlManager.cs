using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class GameUrlManager: EntityUrlManager
    {
        public GameUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper) : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(Game game, bool absolute = false)
        {
            return GetUrl("Index", "Games", new { gameUrl = game.Url }, absolute);
        }

        public string NewsUrl(Game game, bool absolute = false)
        {
            return GetUrl("Game", "News", new { gameUrl = game.Url }, absolute);
        }

        public string ArticlesUrl(Game game, bool absolute = false)
        {
            return GetUrl("Game", "Articles", new { gameUrl = game.Url }, absolute);
        }

        public string FilesUrl(Game game, bool absolute = false)
        {
            return GetUrl("Game", "Files", new { gameUrl = game.Url }, absolute);
        }

        public string GalleryUrl(Game game, bool absolute = false)
        {
            return GetUrl("Game", "Gallery", new { gameUrl = game.Url }, absolute);
        }

        public string LogoUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "big/" + game.Logo;
        }
    }
}
