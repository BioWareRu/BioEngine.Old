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
            var url = CatUrl(article.Cat) + "/" + article.Url;
            return GetUrl("Show", "Articles",
                new {parentUrl = ParentUrl(article), url});
            /*return _urlHelper.Action<ArticlesController>(x => x.Show(ParentUrl(article), CatUrl(article), article.Url));*/
        }

        private static string CatUrl(ArticleCat cat)
        {
            var urls = new SortedList<int, string>();
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
            Poppulate(cat);

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

        private void Poppulate(ArticleCat cat)
        {
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
        }

        public string CatPublicUrl(ArticleCat cat, int page = 1)
        {
            Poppulate(cat);
            var url = CatUrl(cat) + "/" + cat.Url;
            if (page > 1)
            {
                url += $"/page/{page}";
            }
            return GetUrl("Show", "Articles",
                new {parentUrl = ParentUrl(cat), url});
        }

        public string ParentArticlesUrl(Developer developer)
        {
            return UrlHelper.Action<ArticlesController>(x => x.ParentArticles(developer.Url));
        }

        public string ParentArticlesUrl(Game game)
        {
            return UrlHelper.Action<ArticlesController>(x => x.ParentArticles(game.Url));
        }

        public string ParentArticlesUrl(Topic topic)
        {
            return UrlHelper.Action<ArticlesController>(x => x.ParentArticles(topic.Url));
        }
    }
}