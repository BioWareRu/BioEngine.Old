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
    internal class GetCategoryArticlesHandler : ModelListQueryHandlerBase<GetCategoryArticlesQuery, Article>
    {
        public GetCategoryArticlesHandler(HandlerContext<GetCategoryArticlesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<Article>, int)> RunQueryAsync(GetCategoryArticlesQuery message)
        {
            var articlesQuery = DBContext.Articles.Where(x => x.CatId == message.Cat.Id && x.Pub == 1);

            return await GetDataAsync(articlesQuery, message);
        }
    }
}