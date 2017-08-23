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
    internal class GetDevelopersHandler : ModelListQueryHandlerBase<GetDevelopersQuery, Developer>
    {
        public GetDevelopersHandler(HandlerContext<GetDevelopersHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<Developer>, int)> RunQueryAsync(GetDevelopersQuery message)
        {
            return await GetDataAsync(DBContext.Developers.AsQueryable(), message);
        }
    }
}