using System.Collections.Generic;
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
            return await Repository.Gallery.GetCats(
                Mapper.Map<GetGalleryCategoriesQuery, GalleryCatsListQueryOptions>(message));
        }
    }
}