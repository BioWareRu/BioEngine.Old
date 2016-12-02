using BioEngine.Common.DB;
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
        public readonly NewsUrlManager News;

        public UrlManager(BWContext dbContext, IActionContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory, IOptions<AppSettings> options)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            var settings = options.Value;

            Articles = new ArticleUrlManager(settings, dbContext, urlHelper);
            Files = new FileUrlManager(settings, dbContext, urlHelper);
            Gallery = new GalleryUrlManager(settings, dbContext, urlHelper);
            Games = new GameUrlManager(settings, dbContext, urlHelper);
            News = new NewsUrlManager(settings, dbContext, urlHelper);
        }
    }
}