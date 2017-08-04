using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticleByUrlHandler : QueryHandlerBase<GetArticleByUrlQuery, Article>
    {
        public GetArticleByUrlHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Article> Handle(GetArticleByUrlQuery message)
        {
            var query = DBContext.Articles.Include(x => x.Cat).Include(x => x.Author).AsQueryable();
            query = query.Where(x => x.Pub == 1 && x.Url == message.Url);
            query = ApplyParentCondition(query, message.Parent);
            var articles = await query.ToListAsync();
            if (articles.Any())
            {
                Article article = null;
                if (articles.Count > 1)
                {
                    foreach (var candidate in articles)
                    {
                        if (candidate.Cat.Url != message.CatUrl) continue;
                        article = candidate;
                        break;
                    }
                }
                else
                {
                    article = articles[0];
                }
                if (article != null)
                {
                    article.Cat =
                        await Mediator.Send(new ArticleCategoryProcessQuery(article.Cat,
                            new GetArticlesCategoryQuery(message.Parent)));
                    return article;
                }
            }

            return null;
        }
    }
}