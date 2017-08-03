using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    public class PollRecountHandler : NotificationHandlerBase<PollRecountCommand>
    {
        public PollRecountHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task Handle(PollRecountCommand notification)
        {
            var votes = new Dictionary<string, string>();
            foreach (var option in notification.Poll.Options)
            {
                var voteCount =
                    await DBContext.PollVotes.CountAsync(
                        x => x.PollId == notification.Poll.Id && x.VoteOption == option.Id);
                votes.Add($"opt_{option.Id}", voteCount.ToString());
            }
            notification.Poll.Votes = votes;
            await DBContext.SaveChangesAsync();
        }
    }
}