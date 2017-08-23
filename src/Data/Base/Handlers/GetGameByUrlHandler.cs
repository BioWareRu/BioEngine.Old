using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetGameByUrlHandler : QueryHandlerBase<GetGameByUrlQuery, Game>
    {
        public GetGameByUrlHandler(HandlerContext<GetGameByUrlHandler> context) : base(context)
        {
        }

        protected override async Task<Game> RunQueryAsync(GetGameByUrlQuery message)
        {
            return await DBContext.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Url == message.Url);
        }
    }
}