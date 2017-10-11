using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleCatViewModel : BaseViewModel
    {
        public ArticleCatViewModel(BaseViewModelConfig config, ArticleCat articleCat)
            : base(config)
        {
            ArticleCat = articleCat;
        }

        public override string Title()
        {
            var title = ArticleCat.Title;
            title += " - Статьи";
            if (ArticleCat.Parent != null)
                title += " - " + ArticleCat.Parent.DisplayTitle;
            return title;
        }

        protected override Task<string> GetDescription()
        {
            if (!string.IsNullOrEmpty(ArticleCat.Content))
            {
                return Task.FromResult(GetDescriptionFromHtml(ArticleCat.Content));
            }
            return Task.FromResult($"Статьи категории «{ArticleCat.Title}» в разделе «{ArticleCat.Parent?.DisplayTitle}»");
        }

        public ArticleCat ArticleCat { get; }

        public int CurrentPage { get; set; }
        public int TotalArticles { get; set; }
    }
}