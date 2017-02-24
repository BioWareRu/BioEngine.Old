using System;
using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleViewModel : BaseViewModel
    {
        public ArticleViewModel(BaseViewModelConfig config, Article article) : base(config)
        {
            Article = article;
        }

        public override async Task<string> Title()
        {
            var title = Article.Title;
            var parent = await ParentEntityProvider.GetModelParent(Article);
            if (Article.Cat != null)
                title += " - " + Article.Cat.Title;
            if (parent != null)
                title += " - " + parent.DisplayTitle;
            return title;
        }

        public Article Article { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(Article.Date);
    }
}