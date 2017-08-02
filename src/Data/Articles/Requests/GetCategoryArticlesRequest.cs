using System.Collections.Generic;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class GetCategoryArticlesRequest : RequestBase<(IEnumerable<Article> articles, int count)>
    {
        public GetCategoryArticlesRequest(ArticleCat cat, int count = 0)
        {
            Cat = cat;
            Count = count;
        }

        public ArticleCat Cat { get; }
        public int Count { get; }
    }
}