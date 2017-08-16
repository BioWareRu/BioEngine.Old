using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Users.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Users.Handlers
{
    [UsedImplicitly]
    internal class GetUserByIdHandler : QueryHandlerBase<GetUserByIdQuery, User>
    {
        public GetUserByIdHandler(HandlerContext<GetUserByIdHandler> context) : base(context)
        {
        }

        protected override async Task<User> RunQuery(GetUserByIdQuery message)
        {
            return await DBContext.Users.Where(x => x.Id == message.Id).Include(x => x.SiteTeamMember)
                .FirstOrDefaultAsync();
        }
    }
}