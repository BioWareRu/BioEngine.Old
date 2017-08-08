using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class GetArticlesCategoriesQuery : ModelsListQueryBase<ArticleCat>, ICategoryQuery<ArticleCat>
    {
        public bool OnlyRoot { get; set; }
        public IParentModel Parent { get; set; }
        public bool LoadChildren { get; set; }
        public ArticleCat ParentCat { get; set; }
        public int? LoadLastItems { get; set; }
        public string Url { get; set; }
    }
}