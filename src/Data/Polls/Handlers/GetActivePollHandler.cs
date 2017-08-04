using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class GetActivePollHandler : QueryHandlerBase<GetActivePollQuery, Poll>
    {
        public GetActivePollHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Poll> Handle(GetActivePollQuery message)
        {
            return await DBContext.Polls.Where(x => x.OnOff == 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }
    }
}