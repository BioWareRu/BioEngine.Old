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
    internal class GetDevelopersHandler : ModelListQueryHandlerBase<GetDevelopersQuery, Developer>
    {
        public GetDevelopersHandler(IMediator mediator, BWContext dbContext, ILogger<GetDevelopersHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Developer>, int)> RunQuery(GetDevelopersQuery message)
        {
            return await GetData(DBContext.Developers.AsQueryable(), message);
        }
    }
}