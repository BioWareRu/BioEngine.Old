using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleCatViewModel : BaseViewModel
    {
        public ArticleCat ArticleCat { get; }
        public UrlManager UrlManager { get; set; }

        public IEnumerable<CatsTree<ArticleCat,Article>> Children { get; }

        public IEnumerable<Article> LastArticles { get; }

        public ArticleCatViewModel(IEnumerable<Settings> settings, ArticleCat articleCat, IEnumerable<CatsTree<ArticleCat, Article>> children,
            IEnumerable<Article> lastArticles,
            UrlManager urlManager)
            : base(settings)
        {
            ArticleCat = articleCat;
            UrlManager = urlManager;
            Children = children;
            LastArticles = lastArticles;
            var title = articleCat.Title;
            title += " - Статьи";
            if (articleCat.Parent != null)
            {
                title += " - " + articleCat.Parent.DisplayTitle;
            }
            Title = title;
        }

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) ArticleCat.Parent);
        public string ParentArticlesUrl => UrlManager.Articles.ParentArticlesUrl((dynamic) ArticleCat.Parent);
        public int CurrentPage { get; set; }
        public int TotalArticles { get; set; }
    }
}