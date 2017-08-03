using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    public class GetPollByIdHandler : RequestHandlerBase<GetPollByIdRequest, Poll>
    {
        public GetPollByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Poll> Handle(GetPollByIdRequest message)
        {
            return await DBContext.Polls.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}