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
    internal class GetSettingsHandler : ModelListQueryHandlerBase<GetSettingsQuery, Settings>
    {
        public GetSettingsHandler(HandlerContext<GetSettingsHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<Settings>, int)> RunQueryAsync(GetSettingsQuery message)
        {
            return await GetDataAsync(DBContext.Settings.AsQueryable(), message);
        }
    }
}