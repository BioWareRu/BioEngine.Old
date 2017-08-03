using System;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using MediatR;

namespace BioEngine.Data.Polls.Handlers
{
    public class PollVoteHandler : NotificationHandlerBase<PollVoteCommand>
    {
        public PollVoteHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task Handle(PollVoteCommand notification)
        {
            var pollVote = new PollWho
            {
                PollId = notification.PollId,
                VoteDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
                VoteOption = notification.VoteId,
                Ip = notification.IpAddress,
                UserId = notification.UserId,
                Login = notification.Login,
                SessionId = notification.SessionId
            };

            DBContext.Add(pollVote);
            await DBContext.SaveChangesAsync();
        }
    }
}