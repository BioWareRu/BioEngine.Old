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
    internal class GetArticleByIdHandler : QueryHandlerBase<GetArticleByIdQuery, Article>
    {
        public GetArticleByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetArticleByIdHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<Article> RunQuery(GetArticleByIdQuery message)
        {
            var article = await DBContext.Articles
                .Where(x => x.Id == message.Id)
                .Include(x => x.Cat)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic)
                .FirstOrDefaultAsync();
            if (article != null)
            {
                article.Cat =
                    await Mediator.Send(
                        new ArticleCategoryProcessQuery(article.Cat,
                            new GetArticlesCategoriesQuery(article.Parent)));
            }
            return article;
        }
    }
}