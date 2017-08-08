using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class
        ArticlesCategoryProcessHandler : CategoryProcessHandlerBase<ArticleCategoryProcessQuery, ArticleCat, Article>
    {
        public ArticlesCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ILogger<ArticlesCategoryProcessHandler> logger,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, logger, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<Article>> GetCatItems(ArticleCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryArticlesQuery {Cat = cat, PageSize = count})).models;
        }
    }
}