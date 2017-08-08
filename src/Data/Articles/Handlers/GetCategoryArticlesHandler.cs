using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    internal class GetCategoryArticlesHandler : ModelListQueryHandlerBase<GetCategoryArticlesQuery, Article>
    {
        public GetCategoryArticlesHandler(IMediator mediator, BWContext dbContext,
            ILogger<GetCategoryArticlesHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Article>, int)> RunQuery(GetCategoryArticlesQuery message)
        {
            var articlesQuery = DBContext.Articles.Where(x => x.CatId == message.Cat.Id && x.Pub == 1);

            return await GetData(articlesQuery, message);
        }
    }
}