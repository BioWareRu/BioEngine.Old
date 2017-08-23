using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class GetPollByIdHandler : QueryHandlerBase<GetPollByIdQuery, Poll>
    {
        public GetPollByIdHandler(HandlerContext<GetPollByIdHandler> context) : base(context)
        {
        }

        protected override async Task<Poll> RunQueryAsync(GetPollByIdQuery message)
        {
            return await DBContext.Polls.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}