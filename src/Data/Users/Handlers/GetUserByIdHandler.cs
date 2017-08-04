using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Users.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Users.Handlers
{
    [UsedImplicitly]
    internal class GetUserByIdHandler : QueryHandlerBase<GetUserByIdQuery, User>
    {
        public GetUserByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<User> Handle(GetUserByIdQuery message)
        {
            return await DBContext.Users.Where(x => x.Id == message.Id).Include(x => x.SiteTeamMember)
                .FirstOrDefaultAsync();
        }
    }
}