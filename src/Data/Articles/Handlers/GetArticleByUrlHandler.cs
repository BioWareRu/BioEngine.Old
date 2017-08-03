using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    public class GetArticleByUrlHandler : RequestHandlerBase<GetArticleByUrlRequest, Article>
    {
        public GetArticleByUrlHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Article> Handle(GetArticleByUrlRequest message)
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
                        await Mediator.Send(new ArticleCategoryProcessRequest(article.Cat,
                            new GetArticlesCategoryRequest(message.Parent)));
                    return article;
                }
            }

            return null;
        }
    }
}