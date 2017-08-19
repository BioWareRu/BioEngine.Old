using BioEngine.Routing.Articles;
using BioEngine.Routing.Base;
using BioEngine.Routing.Files;
using BioEngine.Routing.Gallery;
using BioEngine.Routing.News;
using BioEngine.Routing.Search;
using JetBrains.Annotations;

namespace BioEngine.Routing
{
    [UsedImplicitly]
    public class BioUrlManager
    {
        public BioUrlManager(BaseUrlManager baseUrlManager, NewsUrlManager newsUrlManager,
            ArticlesUrlManager articlesUrlManager, FilesUrlManager filesUrlManager, GalleryUrlManager galleryUrlManager,
            SearchUrlManager searchUrlManager)
        {
            Base = baseUrlManager;
            News = newsUrlManager;
            Articles = articlesUrlManager;
            Files = filesUrlManager;
            Gallery = galleryUrlManager;
            Search = searchUrlManager;
        }

        public BaseUrlManager Base { get; }

        public NewsUrlManager News { get; }

        public ArticlesUrlManager Articles { get; }

        public FilesUrlManager Files { get; }

        public GalleryUrlManager Gallery { get; }

        public SearchUrlManager Search { get; }
    }
}