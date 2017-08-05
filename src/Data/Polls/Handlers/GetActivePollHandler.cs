using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class GetActivePollHandler : QueryHandlerBase<GetActivePollQuery, Poll>
    {
        public GetActivePollHandler(IMediator mediator, BWContext dbContext, ILogger<GetActivePollHandler> logger) :
            base(mediator, dbContext, logger)
        {
        }

        protected override async Task<Poll> RunQuery(GetActivePollQuery message)
        {
            return await DBContext.Polls.Where(x => x.OnOff == 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }
    }
}