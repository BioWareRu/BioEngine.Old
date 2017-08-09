using BioEngine.Routing.Articles;
using BioEngine.Routing.Base;
using BioEngine.Routing.Files;
using BioEngine.Routing.Gallery;
using BioEngine.Routing.News;
using BioEngine.Routing.Search;

namespace BioEngine.Routing
{
    public class BioUrlManager
    {
        private readonly BaseUrlManager _baseUrlManager;
        private readonly NewsUrlManager _newsUrlManager;
        private readonly ArticlesUrlManager _articlesUrlManager;
        private readonly FilesUrlManager _filesUrlManager;
        private readonly GalleryUrlManager _galleryUrlManager;
        private readonly SearchUrlManager _searchUrlManager;

        public BioUrlManager(BaseUrlManager baseUrlManager, NewsUrlManager newsUrlManager,
            ArticlesUrlManager articlesUrlManager, FilesUrlManager filesUrlManager, GalleryUrlManager galleryUrlManager,
            SearchUrlManager searchUrlManager)
        {
            _baseUrlManager = baseUrlManager;
            _newsUrlManager = newsUrlManager;
            _articlesUrlManager = articlesUrlManager;
            _filesUrlManager = filesUrlManager;
            _galleryUrlManager = galleryUrlManager;
            _searchUrlManager = searchUrlManager;
        }

        public BaseUrlManager Base
        {
            get => _baseUrlManager;
        }

        public NewsUrlManager News
        {
            get => _newsUrlManager;
        }

        public ArticlesUrlManager Articles
        {
            get => _articlesUrlManager;
        }

        public FilesUrlManager Files
        {
            get => _filesUrlManager;
        }

        public GalleryUrlManager Gallery
        {
            get => _galleryUrlManager;
        }

        public SearchUrlManager Search
        {
            get => _searchUrlManager;
        }
    }
}