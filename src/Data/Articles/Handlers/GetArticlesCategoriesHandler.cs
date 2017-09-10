using System.Collections.Generic;
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
    internal class GetArticlesCategoriesHandler : ModelListQueryHandlerBase<GetArticlesCategoriesQuery, ArticleCat>
    {
        public GetArticlesCategoriesHandler(HandlerContext<GetArticlesCategoriesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<ArticleCat>, int)> RunQueryAsync(GetArticlesCategoriesQuery message)
        {
            var query = DBContext.ArticleCats
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic)
                .AsQueryable();
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


            var data = await GetDataAsync(query, message);
            foreach (var cat in data.models)
            {
                message.Parent = cat.Parent;
                await Mediator.Send(new ArticleCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}