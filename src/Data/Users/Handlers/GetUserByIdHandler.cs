using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Users.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Users.Handlers
{
    [UsedImplicitly]
    internal class GetUserByIdHandler : QueryHandlerBase<GetUserByIdQuery, User>
    {
        public GetUserByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetUserByIdHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<User> RunQuery(GetUserByIdQuery message)
        {
            return await DBContext.Users.Where(x => x.Id == message.Id).Include(x => x.SiteTeamMember)
                .FirstOrDefaultAsync();
        }
    }
}