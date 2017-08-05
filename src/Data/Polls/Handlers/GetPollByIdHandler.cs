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
    internal class GetPollByIdHandler : QueryHandlerBase<GetPollByIdQuery, Poll>
    {
        public GetPollByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetPollByIdHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<Poll> RunQuery(GetPollByIdQuery message)
        {
            return await DBContext.Polls.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}