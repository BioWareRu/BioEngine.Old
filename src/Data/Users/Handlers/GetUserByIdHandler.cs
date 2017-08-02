using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Users.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Users.Handlers
{
    public class GetUserByIdHandler : RequestHandlerBase<GetUserByIdRequest, User>
    {
        public GetUserByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<User> Handle(GetUserByIdRequest message)
        {
            return await DBContext.Users.Where(x => x.Id == message.Id).Include(x => x.SiteTeamMember)
                .FirstOrDefaultAsync();
        }
    }
}