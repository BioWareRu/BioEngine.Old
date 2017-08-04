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

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetBannersHandler : QueryHandlerBase<GetBannersQuery, IEnumerable<Advertisement>>
    {
        public GetBannersHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<IEnumerable<Advertisement>> Handle(GetBannersQuery message)
        {
            var currentTs = DateTimeOffset.Now.ToUnixTimeSeconds();
            var banners =
                await DBContext.Advertiesements.Where(
                        x =>
                            x.AdActive == 1 && x.AdLocation == "ad_sidebar" && x.AdStart < currentTs &&
                            (x.AdEnd == 0 || x.AdEnd > currentTs))
                    .ToListAsync();

            return banners;
        }
    }
}