using System;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleViewModel : BaseViewModel
    {
        public ArticleViewModel(BaseViewModelConfig config, Article article) : base(config)
        {
            Article = article;
            var title = article.Title;
            if (article.Cat != null)
                title += " - " + article.Cat.Title;
            if (article.Parent != null)
                title += " - " + article.Parent.DisplayTitle;
            Title = title;
        }

        public Article Article { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(Article.Date);
    }
}