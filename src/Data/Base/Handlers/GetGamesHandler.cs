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
    internal class GetGamesHandler : QueryHandlerBase<GetGamesQuery, IEnumerable<Game>>
    {
        public GetGamesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<IEnumerable<Game>> Handle(GetGamesQuery message)
        {
            return await DBContext.Games.ToListAsync();
        }
    }
}