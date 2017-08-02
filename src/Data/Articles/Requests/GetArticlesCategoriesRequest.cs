using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class GetArticlesCategoriesRequest : RequestBase<IEnumerable<ArticleCat>>, IGetArticlesCategoryRequest
    {
        public GetArticlesCategoriesRequest(IParentModel parent = null, ArticleCat parentCat = null,
            bool loadChildren = false, int? loadLastItems = null, bool onlyRoot = false)
        {
            Parent = parent;
            ParentCat = parentCat;
            LoadChildren = loadChildren;
            LoadLastItems = loadLastItems;
            OnlyRoot = onlyRoot;
        }

        public bool OnlyRoot { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public ArticleCat ParentCat { get; }
        public int? LoadLastItems { get; }
    }
}