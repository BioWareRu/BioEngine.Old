using System;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class PollVoteHandler : CommandHandlerBase<PollVoteCommand>
    {
        public PollVoteHandler(HandlerContext<PollVoteHandler> context) : base(context)
        {
        }

        protected override async Task ExecuteCommand(PollVoteCommand command)
        {
            var pollVote = new PollWho
            {
                PollId = command.PollId,
                VoteDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
                VoteOption = command.VoteId,
                Ip = command.IpAddress,
                UserId = command.UserId,
                Login = command.Login,
                SessionId = command.SessionId
            };

            DBContext.Add(pollVote);
            await DBContext.SaveChangesAsync();
        }
    }
}