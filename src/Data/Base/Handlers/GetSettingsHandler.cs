using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetSettingsHandler : ModelListQueryHandlerBase<GetSettingsQuery, Settings>
    {
        public GetSettingsHandler(IMediator mediator, BWContext dbContext, ILogger<GetSettingsHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Settings>, int)> RunQuery(GetSettingsQuery message)
        {
            return await GetData(DBContext.Settings.AsQueryable(), message);
        }
    }
}