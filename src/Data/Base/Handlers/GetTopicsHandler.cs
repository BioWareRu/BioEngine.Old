using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetTopicsHandler : ModelListQueryHandlerBase<GetTopicsQuery, Topic>
    {
        public GetTopicsHandler(IMediator mediator, BWContext dbContext, ILogger<GetTopicsHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Topic>, int)> RunQuery(GetTopicsQuery message)
        {
            return await GetData(DBContext.Topics.AsQueryable(), message);
        }
    }
}