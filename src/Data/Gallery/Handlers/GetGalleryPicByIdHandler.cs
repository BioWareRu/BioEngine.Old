﻿using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryPicByIdHandler : QueryHandlerBase<GetGalleryPicByIdQuery, GalleryPic>
    {
        public GetGalleryPicByIdHandler(HandlerContext<GetGalleryPicByIdHandler> context) : base(context)
        {
        }

        protected override async Task<GalleryPic> RunQuery(GetGalleryPicByIdQuery message)
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
                    await Mediator.Send(new GalleryCategoryProcessQuery(pic.Cat,
                        new GetGalleryCategoryQuery()));

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