using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Polls.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Polls.Handlers
{
    [UsedImplicitly]
    internal class GetActivePollHandler : QueryHandlerBase<GetActivePollQuery, Poll>
    {
        public GetActivePollHandler(HandlerContext<GetActivePollHandler> context) : base(context)
        {
        }

        protected override async Task<Poll> RunQuery(GetActivePollQuery message)
        {
            return await DBContext.Polls.Where(x => x.OnOff == 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }
    }
}