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
    internal class GetGameByIdHandler : QueryHandlerBase<GetGameByIdQuery, Game>
    {
        public GetGameByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetGameByIdHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<Game> RunQuery(GetGameByIdQuery message)
        {
            return await DBContext.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}