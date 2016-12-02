using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class ArticleUrlManager : EntityUrlManager
    {
        public ArticleUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
            : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(Article article)
        {
            Poppulate(article);
            return GetUrl("Show", "Articles",
                new {parentUrl = ParentUrl(article), catUrl = CatUrl(article), articleUrl = article.Url});
            /*return _urlHelper.Action<ArticlesController>(x => x.Show(ParentUrl(article), CatUrl(article), article.Url));*/
        }

        private static string CatUrl(Article article)
        {
            var urls = new SortedList<int, string>();
            var cat = article.Cat;
            var i = 0;
            while (cat != null)
            {
                urls.Add(i, cat.Url);
                i++;
                cat = cat.ParentCat;
            }
            return string.Join("/", urls.Reverse().Select(x => x.Value).ToArray());
        }

        private void Poppulate(Article article)
        {
            if (article.Cat == null)
            {
                DbContext.Entry(article).Reference(x => x.Cat).Load();
            }
            var cat = article.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                {
                    if (cat.Pid > 0)
                    {
                        DbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    }
                    else
                    {
                        break;
                    }
                }
                cat = cat.ParentCat;
            }

            if (article.GameId > 0 && article.Game == null)
            {
                DbContext.Entry(article).Reference(x => x.Game).Load();
            }
            if (article.DeveloperId > 0 && article.Developer == null)
            {
                DbContext.Entry(article).Reference(x => x.Developer).Load();
            }
            if (article.TopicId > 0 && article.Topic == null)
            {
                DbContext.Entry(article).Reference(x => x.Topic).Load();
            }
        }
    }
}