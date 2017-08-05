using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class PollRecountHandler : CommandHandlerBase<PollRecountCommand>
    {
        public PollRecountHandler(IMediator mediator, BWContext dbContext, ILogger<PollRecountHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task ExecuteCommand(PollRecountCommand command)
        {
            var votes = new Dictionary<string, string>();
            foreach (var option in command.Poll.Options)
            {
                var voteCount =
                    await DBContext.PollVotes.CountAsync(
                        x => x.PollId == command.Poll.Id && x.VoteOption == option.Id);
                votes.Add($"opt_{option.Id}", voteCount.ToString());
            }
            command.Poll.Votes = votes;
            await DBContext.SaveChangesAsync();
        }
    }
}