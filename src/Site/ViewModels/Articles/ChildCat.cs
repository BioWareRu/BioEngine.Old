using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public struct ChildCat
    {
        public ArticleCat Cat { get; }
        public IReadOnlyCollection<Article> LastArticles { get; }

        public List<ChildCat> Children { get; }

        public ChildCat(ArticleCat cat, List<Article> lastArticles, List<ChildCat> children = null)
        {
            Cat = cat;
            LastArticles = lastArticles.AsReadOnly();
            Children = children;
        }
    }
}