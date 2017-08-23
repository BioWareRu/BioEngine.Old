using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

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


            query =
                query
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Cat);

            var data = await GetDataAsync(query, message);

            foreach (var pic in data.models)
            {
                pic.Cat =
                    await Mediator.Send(new GalleryCategoryProcessQuery(pic.Cat,
                        new GetGalleryCategoryQuery {Parent = message.Parent}));

                if (message.LoadPicPositions)
                {
                    pic.Position = await GetPicPosition(pic);
                }
            }

            return data;
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