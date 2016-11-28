using BioEngine.Common.Models;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components
{
    public class UrlManager
    {
        private readonly AppSettings _settings;

        public UrlManager(IOptions<AppSettings> options)
        {
            _settings = options.Value;
        }

        public string GetParentIconUrl(Developer developer)
        {
            return _settings.AssetsDomain
                   + _settings.DevelopersImagesPath + developer.Icon;
        }

        public string GetParentIconUrl(Game game)
        {
            return _settings.AssetsDomain + _settings.GamesImagesPath + "small/" + game.Icon;
        }

        public string GetParentIconUrl(Topic topic)
        {
            return _settings.AssetsDomain + _settings.TopicsImagesPath + topic.Icon;
        }
    }
}