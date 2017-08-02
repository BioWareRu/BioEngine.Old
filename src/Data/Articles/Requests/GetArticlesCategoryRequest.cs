using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class GetArticlesCategoryRequest : RequestBase<ArticleCat>, IGetArticlesCategoryRequest
    {
        public GetArticlesCategoryRequest(ArticleCat parentCat = null, IParentModel parent = null, string url = null,
            bool loadChildren = false,
            int? loadLastItems = null)
        {
            Url = url;
            Parent = parent;
            LoadChildren = loadChildren;
            ParentCat = parentCat;
            LoadLastItems = loadLastItems;
        }

        public string Url { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public ArticleCat ParentCat { get; }
        public int? LoadLastItems { get; }
    }
}