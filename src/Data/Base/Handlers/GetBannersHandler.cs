using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class
        GetBannersHandler : QueryHandlerBase<GetBannersQuery, (IEnumerable<Advertisement>, int)>
    {
        public GetBannersHandler(IMediator mediator, BWContext dbContext, ILogger<GetBannersHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Advertisement>, int)> RunQuery(
            GetBannersQuery message)
        {
            var currentTs = DateTimeOffset.Now.ToUnixTimeSeconds();
            var banners =
                await DBContext.Advertiesements.Where(
                        x =>
                            x.AdActive == 1 && x.AdLocation == "ad_sidebar" && x.AdStart < currentTs &&
                            (x.AdEnd == 0 || x.AdEnd > currentTs))
                    .ToListAsync();

            return (banners, banners.Count);
        }
    }
}