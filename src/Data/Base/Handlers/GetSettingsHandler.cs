using System.Collections.Generic;
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
    internal class GetSettingsHandler : QueryHandlerBase<GetSettingsQuery, List<Settings>>
    {
        public GetSettingsHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<List<Settings>> Handle(GetSettingsQuery message)
        {
            return await DBContext.Settings.ToListAsync();
        }
    }
}