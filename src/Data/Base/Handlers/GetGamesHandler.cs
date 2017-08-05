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
    internal class GetGamesHandler : QueryHandlerBase<GetGamesQuery, IEnumerable<Game>>
    {
        public GetGamesHandler(IMediator mediator, BWContext dbContext, ILogger<GetGamesHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<IEnumerable<Game>> RunQuery(GetGamesQuery message)
        {
            return await DBContext.Games.ToListAsync();
        }
    }
}