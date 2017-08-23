using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class PollRecountHandler : CommandHandlerBase<PollRecountCommand>
    {
        public PollRecountHandler(HandlerContext<PollRecountHandler> context) : base(context)
        {
        }

        protected override async Task ExecuteCommandAsync(PollRecountCommand command)
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