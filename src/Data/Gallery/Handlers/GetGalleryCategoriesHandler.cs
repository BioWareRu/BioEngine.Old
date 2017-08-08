using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class
        GetGalleryCategoriesHandler : ModelListQueryHandlerBase<GetGalleryCategoriesQuery, GalleryCat>
    {
        public GetGalleryCategoriesHandler(IMediator mediator, BWContext dbContext,
            ILogger<GetGalleryCategoriesHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<GalleryCat>, int)> RunQuery(GetGalleryCategoriesQuery message)
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

            var data = await GetData(query, message);
            foreach (var cat in data.models)
            {
                await Mediator.Send(new GalleryCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}