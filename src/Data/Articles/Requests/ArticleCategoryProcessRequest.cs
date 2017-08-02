using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class ArticleCategoryProcessRequest : CategoryProcessRequestBase<ArticleCat>
    {
        public ArticleCategoryProcessRequest(ArticleCat cat, ICategoryRequest<ArticleCat> request) : base(cat, request)
        {
        }
    }
}