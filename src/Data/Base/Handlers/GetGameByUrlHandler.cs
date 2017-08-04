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
    internal class GetGameByUrlHandler : QueryHandlerBase<GetGameByUrlQuery, Game>
    {
        public GetGameByUrlHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Game> Handle(GetGameByUrlQuery message)
        {
            return await DBContext.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Url == message.Url);
        }
    }
}