using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryPicsHandler : ModelListQueryHandlerBase<GetGalleryPicsQuery, GalleryPic>
    {
        public GetGalleryPicsHandler(HandlerContext<GetGalleryPicsHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<GalleryPic>, int)> RunQueryAsync(GetGalleryPicsQuery message)
        {
            return await Repository.Gallery.GetPics(
                Mapper.Map<GetGalleryPicsQuery, GalleryPicsListQueryOptions>(message));
        }
    }
}