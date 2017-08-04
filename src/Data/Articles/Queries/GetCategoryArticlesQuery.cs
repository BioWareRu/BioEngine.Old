using System.Collections.Generic;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class GetCategoryArticlesQuery : QueryBase<(IEnumerable<Article> articles, int count)>
    {
        public GetCategoryArticlesQuery(ArticleCat cat, int page = 1)
        {
            Cat = cat;
            Page = page;
        }

        public int PageSize { get; set; } = 20;

        public ArticleCat Cat { get; }
        public int Page { get; }
    }
}