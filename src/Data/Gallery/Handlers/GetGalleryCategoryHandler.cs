using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryCategoryHandler : QueryHandlerBase<GetGalleryCategoryQuery, GalleryCat>
    {
        public GetGalleryCategoryHandler(IMediator mediator, BWContext dbContext,
            ILogger<GetGalleryCategoryHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<GalleryCat> RunQuery(GetGalleryCategoryQuery message)
        {
            var catQuery = DBContext.GalleryCats.AsQueryable();

            if (!string.IsNullOrEmpty(message.Url))
            {
                catQuery = catQuery.Where(x => x.Url == message.Url);
            }

            if (message.Parent != null)
            {
                catQuery = ApplyParentCondition(catQuery, message.Parent);
            }

            if (message.ParentCat != null)
            {
                catQuery = catQuery.Where(x => x.Pid == message.ParentCat.Id);
            }
            else
            {
                catQuery = catQuery.Include(x => x.ParentCat);
            }

            var cat = await catQuery.FirstOrDefaultAsync();
            if (cat != null)
            {
                cat = await Mediator.Send(new GalleryCategoryProcessQuery(cat, message));
            }
            return cat;
        }
    }
}