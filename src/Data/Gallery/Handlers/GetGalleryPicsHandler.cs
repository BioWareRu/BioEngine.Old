using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery.Handlers
{
    class GetGalleryPicsHandler : RequestHandlerBase<GetGalleryPicsRequest, (IEnumerable<Common.Models.GalleryPic>
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
            var totalPics = await query.CountAsync();
            var pics =
                await query
                    .OrderByDescending(x => x.Id)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Include(x => x.Cat)
                    .Skip((message.Page - 1) * message.PageSize)
                    .Take(message.PageSize)
                    .ToListAsync();

            foreach (var article in pics)
            {
                article.Cat =
                    await Mediator.Send(new GalleryCategoryProcessRequest(article.Cat,
                        new GetGalleryCategoryRequest(message.Parent)));
            }

            return (pics, totalPics);
        }
    }
}