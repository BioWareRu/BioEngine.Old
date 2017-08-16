using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticlesCategoriesHandler : ModelListQueryHandlerBase<GetArticlesCategoriesQuery, ArticleCat>
    {
        public GetArticlesCategoriesHandler(HandlerContext<GetArticlesCategoriesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<ArticleCat>, int)> RunQuery(GetArticlesCategoriesQuery message)
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


            var data = await GetData(query, message);
            foreach (var cat in data.models)
            {
                await Mediator.Send(new ArticleCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}