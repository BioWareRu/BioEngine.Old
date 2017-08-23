using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetGamesHandler : ModelListQueryHandlerBase<GetGamesQuery, Game>
    {
        public GetGamesHandler(HandlerContext<GetGamesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<Game>, int)> RunQueryAsync(GetGamesQuery message)
        {
            return await GetDataAsync(DBContext.Games.AsQueryable(), message);
        }
    }
}