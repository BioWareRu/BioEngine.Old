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
    internal class GetGameByUrlHandler : QueryHandlerBase<GetGameByUrlQuery, Game>
    {
        public GetGameByUrlHandler(IMediator mediator, BWContext dbContext, ILogger<GetGameByUrlHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<Game> RunQuery(GetGameByUrlQuery message)
        {
            return await DBContext.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Url == message.Url);
        }
    }
}