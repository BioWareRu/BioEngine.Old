using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticlesCategoriesHandler : QueryHandlerBase<GetArticlesCategoriesQuery,
        IEnumerable<ArticleCat>>
    {
        public GetArticlesCategoriesHandler(IMediator mediator, BWContext dbContext,
            ILogger<GetArticlesCategoriesHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<IEnumerable<ArticleCat>> RunQuery(GetArticlesCategoriesQuery message)
        {
            var query = DBContext.ArticleCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.Pid == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.Pid == null);
            }


            var cats = await query.ToListAsync();
            foreach (var cat in cats)
            {
                await Mediator.Send(new ArticleCategoryProcessQuery(cat, message));
            }

            return cats;
        }
    }
}