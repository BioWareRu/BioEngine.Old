using System.Collections.Generic;
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
    internal class GetSettingsHandler : QueryHandlerBase<GetSettingsQuery, List<Settings>>
    {
        public GetSettingsHandler(IMediator mediator, BWContext dbContext, ILogger<GetSettingsHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<List<Settings>> RunQuery(GetSettingsQuery message)
        {
            return await DBContext.Settings.ToListAsync();
        }
    }
}