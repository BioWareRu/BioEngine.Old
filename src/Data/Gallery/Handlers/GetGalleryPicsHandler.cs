using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Handlers;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryPicsHandler : QueryHandlerBase<GetGalleryPicsQuery, (
        IEnumerable<GalleryPic>
        pics, int count)>
    {
        public GetGalleryPicsHandler(IMediator mediator, BWContext dbContext, ILogger<GetGalleryPicsHandler> logger) :
            base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<GalleryPic> pics, int count)> RunQuery(
            GetGalleryPicsQuery message)
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
                    await Mediator.Send(new GalleryCategoryProcessQuery(pic.Cat,
                        new GetGalleryCategoryQuery(message.Parent)));

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