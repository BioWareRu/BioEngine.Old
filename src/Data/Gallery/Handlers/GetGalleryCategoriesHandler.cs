using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryCategoriesHandler : QueryHandlerBase<GetGalleryCategoriesQuery, IEnumerable<GalleryCat>>
    {
        public GetGalleryCategoriesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<IEnumerable<GalleryCat>> Handle(GetGalleryCategoriesQuery message)
        {
            var query = DBContext.GalleryCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.Pid == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.Pid == null);
            }


            var cats = await query.ToListAsync();
            foreach (var cat in cats)
            {
                await Mediator.Send(new GalleryCategoryProcessQuery(cat, message));
            }

            return cats;
        }
    }
}