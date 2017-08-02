using System.Collections.Generic;
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
    public class GetCategoryArticlesHandler : RequestHandlerBase<GetCategoryArticlesRequest, (IEnumerable<Article>
        articles, int count)>
    {
        public GetCategoryArticlesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<(IEnumerable<Article> articles, int count)> Handle(
            GetCategoryArticlesRequest message)
        {
            var articlesQuery = DBContext.Articles.Where(x => x.CatId == message.Cat.Id && x.Pub == 1)
                .OrderByDescending(x => x.Id).AsQueryable();

            if (message.Count > 0)
            {
                articlesQuery = articlesQuery.Take(message.Count);
            }

            var articles = await articlesQuery.ToListAsync();
            var count = await DBContext.Articles.CountAsync(x => x.CatId == message.Cat.Id);

            return (articles, count);
        }
    }
}