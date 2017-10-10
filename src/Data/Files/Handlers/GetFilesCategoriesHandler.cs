using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFilesCategoriesHandler : ModelListQueryHandlerBase<GetFilesCategoriesQuery, FileCat>
    {
        public GetFilesCategoriesHandler(HandlerContext<GetFilesCategoriesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<FileCat>, int)> RunQueryAsync(GetFilesCategoriesQuery message)
        {
            var query = DBContext.FileCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.CatId == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.CatId == null);
            }

            var data = await GetDataAsync(query, message);

            foreach (var cat in data.models)
            {
                await Mediator.Send(new FileCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}