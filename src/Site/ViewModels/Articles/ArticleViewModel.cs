using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ArticleViewModel : BaseViewModel
    {
        public Article Article { get; }

        public ArticleViewModel(IEnumerable<Settings> settings, Article article) : base(settings)
        {
            Article = article;
            var title = article.Title;
            if (article.Cat != null)
            {
                title += " - " + article.Cat.Title;
            }
            if (article.Parent != null)
            {
                title += " - " + article.Parent.DisplayTitle;
            }
            Title = title;
        }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(Article.Date);
    }
}
