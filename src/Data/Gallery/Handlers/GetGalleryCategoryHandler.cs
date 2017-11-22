using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryCategoryHandler : QueryHandlerBase<GetGalleryCategoryQuery, GalleryCat>
    {
        public GetGalleryCategoryHandler(HandlerContext<GetGalleryCategoryHandler> context) : base(context)
        {
        }

        protected override async Task<GalleryCat> RunQueryAsync(GetGalleryCategoryQuery message)
        {
            return await Repository.Gallery.GetCat(
                Mapper.Map<GetGalleryCategoryQuery, GalleryCatsListQueryOptions>(message));
        }
    }
}