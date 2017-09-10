using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class GetArticleCategoryByIdQuery : SingleModelQueryBase<ArticleCat>, ICategoryQuery<ArticleCat>
    {
        public GetArticleCategoryByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public ArticleCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}