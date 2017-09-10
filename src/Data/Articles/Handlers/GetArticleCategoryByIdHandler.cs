using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticleCategoryByIdHandler : QueryHandlerBase<GetArticleCategoryByIdQuery, ArticleCat>
    {
        public GetArticleCategoryByIdHandler(HandlerContext<GetArticleCategoryByIdHandler> context) : base(context)
        {
        }

        protected override async Task<ArticleCat> RunQueryAsync(GetArticleCategoryByIdQuery message)
        {
            var cat = await DBContext.ArticleCats
                .Where(x => x.Id == message.Id)
                .Include(x => x.ParentCat)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic)
                .FirstOrDefaultAsync();
            if (cat != null)
            {
                cat = await Mediator.Send(new ArticleCategoryProcessQuery(cat, message));
            }
            return cat;
        }
    }
}