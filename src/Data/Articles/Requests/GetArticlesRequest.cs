﻿using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class GetArticlesRequest : RequestBase<(IEnumerable<Common.Models.Article> articles, int count)>
    {
        public bool WithUnPublishedArticles { get; }
        public int? Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public GetArticlesRequest(bool withUnPublishedArticles = false, int? page = null, IParentModel parent = null)
        {
            WithUnPublishedArticles = withUnPublishedArticles;
            Page = page;
            Parent = parent;
        }
    }
}