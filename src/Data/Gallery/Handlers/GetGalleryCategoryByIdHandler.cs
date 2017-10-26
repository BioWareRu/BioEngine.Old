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
    internal class GetGalleryCategoryByIdHandler : QueryHandlerBase<GetGalleryCategoryByIdQuery, GalleryCat>
    {
        public GetGalleryCategoryByIdHandler(HandlerContext<GetGalleryCategoryByIdQuery> context) : base(context)
        {
        }

        protected override async Task<GalleryCat> RunQueryAsync(GetGalleryCategoryByIdQuery message)
        {
            var cat = await DBContext.GalleryCats
                .Where(x => x.Id == message.Id)
                .Include(x => x.ParentCat)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .FirstOrDefaultAsync();
            if (cat != null)
            {
                cat = await Mediator.Send(new GalleryCategoryProcessQuery(cat, message));
            }
            return cat;
        }
    }
}