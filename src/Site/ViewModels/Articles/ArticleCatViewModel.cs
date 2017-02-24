using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleCatViewModel : BaseViewModel
    {
        public ArticleCatViewModel(BaseViewModelConfig config, ArticleCat articleCat,
            IEnumerable<CatsTree<ArticleCat, Article>> children,
            IEnumerable<Article> lastArticles)
            : base(config)
        {
            ArticleCat = articleCat;
            Children = children;
            LastArticles = lastArticles;
        }

        public override async Task<string> Title()
        {
            var parent = await ParentEntityProvider.GetModelParent(ArticleCat);
            var title = ArticleCat.Title;
            title += " - Статьи";
            if (parent != null)
                title += " - " + parent.DisplayTitle;
            return title;
        }

        public ArticleCat ArticleCat { get; }

        public IEnumerable<CatsTree<ArticleCat, Article>> Children { get; }

        public IEnumerable<Article> LastArticles { get; }

        public int CurrentPage { get; set; }
        public int TotalArticles { get; set; }
    }
}