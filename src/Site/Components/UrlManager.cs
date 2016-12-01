using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components
{
    public class UrlManager
    {
        private readonly AppSettings _settings;
        private readonly IUrlHelper _urlHelper;

        public readonly ArticleUrlManager Articles;
        public readonly FileUrlManager Files;
        public readonly GalleryUrlManager Gallery;
        public readonly GameUrlManager Games;
        public readonly NewsUrlManager News;

        public UrlManager(BWContext dbContext, IActionContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory, IOptions<AppSettings> options)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            _settings = options.Value;

            Articles = new ArticleUrlManager(_settings, dbContext, _urlHelper);
            Files = new FileUrlManager(_settings, dbContext, _urlHelper);
            Gallery = new GalleryUrlManager(_settings, dbContext, _urlHelper);
            Games = new GameUrlManager(_settings, dbContext, _urlHelper);
            News = new NewsUrlManager(_settings, dbContext, _urlHelper);
        }
    }
}