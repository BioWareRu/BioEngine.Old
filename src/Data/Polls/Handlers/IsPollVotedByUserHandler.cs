using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class IsPollVotedByUserHandler : QueryHandlerBase<IsPollVotedByUserQuery, bool>
    {
        public IsPollVotedByUserHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<bool> Handle(IsPollVotedByUserQuery message)
        {
            if (message.UserId > 0)
            {
                return await
                    DBContext.PollVotes.AnyAsync(x => x.UserId == message.UserId && x.PollId == message.PollId);
            }
            return await
                DBContext.PollVotes.AnyAsync(x => x.UserId == 0 && x.PollId == message.PollId &&
                                                  x.Ip == message.IpAddress &&
                                                  x.SessionId == message.SessionId);
        }
    }
}