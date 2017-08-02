using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Articles
{
    public class ArticlesUrlManager : UrlManager<ArticlesRoutesEnum>
    {
        public ArticlesUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper,
            appSettings)
        {
        }

        public string PublicUrl(Article article, bool absoluteUrl = false)
        {
            var url = CatUrl(article.Cat) + "/" + article.Url;
            return GetUrl(ArticlesRoutesEnum.ArticlePage, new {parentUrl = article.Parent.ParentUrl, url}, absoluteUrl);
        }


        public string CatPublicUrl(ArticleCat cat, int page = 1)
        {
            var url = CatUrl(cat) + "/" + cat.Url;
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl(ArticlesRoutesEnum.ArticlePage, new {parentUrl = cat.Parent.ParentUrl, url});
        }

        public string ParentArticlesUrl(IChildModel childModel, bool absolute = false)
        {
            return ParentArticlesUrl(childModel.Parent, absolute);
        }

        public string ParentArticlesUrl(IParentModel parentModel, bool absolute = false)
        {
            return GetUrl(ArticlesRoutesEnum.ArticlesByParent, new {parentUrl = parentModel.ParentUrl}, absolute);
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
    }
}