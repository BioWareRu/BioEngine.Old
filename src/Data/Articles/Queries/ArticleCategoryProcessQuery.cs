using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class ArticleCategoryProcessQuery : CategoryProcessQueryBase<ArticleCat>
    {
        public ArticleCategoryProcessQuery(ArticleCat cat, ICategoryQuery<ArticleCat> query) : base(cat, query)
        {
        }
    }
}