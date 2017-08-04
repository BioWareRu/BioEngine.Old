using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class PollRecountHandler : CommandHandlerBase<PollRecountCommand>
    {
        public PollRecountHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task Handle(PollRecountCommand command)
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