using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryCategoriesHandler :
        ModelListQueryHandlerBase<GetGalleryCategoriesQuery, GalleryCat>
    {
        public GetGalleryCategoriesHandler(HandlerContext<GetGalleryCategoriesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<GalleryCat>, int)> RunQueryAsync(GetGalleryCategoriesQuery message)
        {
            var query = DBContext.GalleryCats.AsQueryable();
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
                await Mediator.Send(new GalleryCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}