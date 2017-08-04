using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Requests;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    public class GetGalleryPicByIdHandler : RequestHandlerBase<GetGalleryPicByIdRequest, GalleryPic>
    {
        public GetGalleryPicByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<GalleryPic> Handle(GetGalleryPicByIdRequest message)
        {
            var pic =
                await DBContext.GalleryPics.Where(x => x.Id == message.Id)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Cat)
                    .FirstOrDefaultAsync();
            if (pic != null)
            {
                pic.Cat =
                    await Mediator.Send(new GalleryCategoryProcessRequest(pic.Cat,
                        new GetGalleryCategoryRequest()));

                pic.Position = await GetPicPosition(pic);
            }

            return pic;
        }

        private async Task<int> GetPicPosition(GalleryPic picture)
        {
            return
                await DBContext.GalleryPics.Where(x => x.CatId == picture.CatId && x.Pub == 1 && x.Id > picture.Id)
                    .OrderByDescending(x => x.Id)
                    .CountAsync();
        }
    }
}