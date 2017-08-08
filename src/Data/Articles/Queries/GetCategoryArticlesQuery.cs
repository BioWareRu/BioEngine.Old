using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class GetCategoryArticlesQuery : ModelsListQueryBase<Article>
    {
        public ArticleCat Cat { get; set; }
    }
}