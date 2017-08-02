using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Data.Articles.Requests
{
    public interface IGetArticlesCategoryRequest
    {
        IParentModel Parent { get; }

        bool LoadChildren { get; }

        ArticleCat ParentCat { get; }

        int? LoadLastItems { get; }
    }
}