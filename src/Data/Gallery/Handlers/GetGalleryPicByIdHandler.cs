using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryPicByIdHandler : QueryHandlerBase<GetGalleryPicByIdQuery, GalleryPic>
    {
        public GetGalleryPicByIdHandler(HandlerContext<GetGalleryPicByIdHandler> context) : base(context)
        {
        }

        protected override async Task<GalleryPic> RunQueryAsync(GetGalleryPicByIdQuery message)
        {
            return await Repository.Gallery.GetById(message.Id);
        }
    }
}