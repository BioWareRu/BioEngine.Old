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
    internal class GetDeveloperByIdHandler : QueryHandlerBase<GetDeveloperByIdQuery, Developer>
    {
        public GetDeveloperByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetDeveloperByIdHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<Developer> RunQuery(GetDeveloperByIdQuery message)
        {
            return await DBContext.Developers.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}