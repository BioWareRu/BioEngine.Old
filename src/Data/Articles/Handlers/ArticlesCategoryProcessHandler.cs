using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Data.Base;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class
        ArticlesCategoryProcessHandler : CategoryProcessHandlerBase<ArticleCategoryProcessQuery, ArticleCat, Article>
    {
        public ArticlesCategoryProcessHandler(HandlerContext<ArticlesCategoryProcessHandler> context,
            ParentEntityProvider parentEntityProvider) : base(context, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<Article>> GetCatItemsAsync(ArticleCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryArticlesQuery {Cat = cat, PageSize = count})).models;
        }
    }
}