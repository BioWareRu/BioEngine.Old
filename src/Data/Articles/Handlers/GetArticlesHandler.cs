using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    public class GetArticlesHandler : RequestHandlerBase<GetArticlesRequest, (IEnumerable<Common.Models.Article>
        articles, int count)>
    {
        public GetArticlesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<(IEnumerable<Common.Models.Article> articles, int count)> Handle(
            GetArticlesRequest message)
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
                query = query.Skip(((int)message.Page - 1) * message.PageSize)
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
                    await Mediator.Send(new ArticleCategoryProcessRequest(article.Cat,
                        new GetArticlesCategoryRequest(message.Parent)));
            }

            return (articles, totalArticles);
        }
    }
}