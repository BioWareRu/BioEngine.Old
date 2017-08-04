using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery.Handlers
{
    public class GetGalleryPicsHandler : RequestHandlerBase<GetGalleryPicsRequest, (
        IEnumerable<Common.Models.GalleryPic>
        pics, int count)>
    {
        public GetGalleryPicsHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<(IEnumerable<Common.Models.GalleryPic> pics, int count)> Handle(
            GetGalleryPicsRequest message)
        {
            var query = DBContext.GalleryPics.AsQueryable();
            if (!message.WithUnPublishedPictures)
                query = query.Where(x => x.Pub == 1);
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }
            if (message.Cat != null)
            {
                query = query.Where(x => x.CatId == message.Cat.Id);
            }
            var totalPics = await query.CountAsync();

            if (message.Page != null && message.Page > 0)
            {
                query = query.Skip(((int) message.Page - 1) * message.PageSize)
                    .Take(message.PageSize);
            }

            var pics =
                await query
                    .OrderByDescending(x => x.Id)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Cat)
                    .ToListAsync();

            foreach (var pic in pics)
            {
                pic.Cat =
                    await Mediator.Send(new GalleryCategoryProcessRequest(pic.Cat,
                        new GetGalleryCategoryRequest(message.Parent)));

                if (message.LoadPicPositions)
                {
                    pic.Position = await GetPicPosition(pic);
                }
            }

            return (pics, totalPics);
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