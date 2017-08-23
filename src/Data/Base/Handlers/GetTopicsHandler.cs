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
    internal class GetTopicsHandler : ModelListQueryHandlerBase<GetTopicsQuery, Topic>
    {
        public GetTopicsHandler(HandlerContext<GetTopicsHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<Topic>, int)> RunQueryAsync(GetTopicsQuery message)
        {
            return await GetDataAsync(DBContext.Topics.AsQueryable(), message);
        }
    }
}