using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    public class IsPollVotedByUserHandler : RequestHandlerBase<IsPollVotedByUserRequest, bool>
    {
        public IsPollVotedByUserHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<bool> Handle(IsPollVotedByUserRequest message)
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