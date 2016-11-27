using BioEngine.Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.Components
{
    public class UrlManager
    {
        private AppSettings Settings;

        public UrlManager(IOptions<AppSettings> options)
        {
            Settings = options.Value;
        }

        public string GetParentIconUrl(Developer developer)
        {
            return Settings.AssetsDomain
                 + Settings.DevelopersImagesPath + developer.Icon;
        }

        public string GetParentIconUrl(Game game)
        {
            return Settings.AssetsDomain + Settings.GamesImagesPath + "small/" + game.Icon;
        }

        public string GetParentIconUrl(Topic topic)
        {
            return Settings.AssetsDomain + Settings.TopicsImagesPath + topic.Icon;
        }
    }
}
