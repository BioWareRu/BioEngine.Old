using System;
using System.Linq;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Queries
{
    public class GetArticlesQuery : ModelsListQueryBase<Article>
    {
        public bool WithUnPublishedArticles { get; set; }
        public IParentModel Parent { get; set; }

        public override Func<IQueryable<Article>, IQueryable<Article>> OrderByFunc { get; protected set; } =
            query => query.OrderByDescending(x => x.Id);
    }
}