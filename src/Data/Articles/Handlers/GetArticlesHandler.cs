using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticlesHandler : QueryHandlerBase<GetArticlesQuery, (IEnumerable<Common.Models.Article>
        articles, int count)>
    {
        public GetArticlesHandler(IMediator mediator, BWContext dbContext, ILogger<GetArticlesHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Common.Models.Article> articles, int count)> RunQuery(
            GetArticlesQuery message)
        {
            var query = DBContext.Articles.AsQueryable();
            if (!message.WithUnPublishedArticles)
                query = query.Where(x => x.Pub == 1);
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }
            var totalArticles = await query.CountAsync();
            if (message.Page != null && message.Page > 0)
            {
                query = query.Skip(((int) message.Page - 1) * message.PageSize)
                    .Take(message.PageSize);
            }

            var articles =
                await query
                    .OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Include(x => x.Cat)
                    .ToListAsync();

            foreach (var article in articles)
            {
                article.Cat =
                    await Mediator.Send(new ArticleCategoryProcessQuery(article.Cat,
                        new GetArticlesCategoryQuery(message.Parent)));
            }

            return (articles, totalArticles);
        }
    }
}