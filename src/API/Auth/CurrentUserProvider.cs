using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Users.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BioEngine.API.Auth
{
    [UsedImplicitly]
    public class CurrentUserProvider
    {
        private readonly ActionContext _context;
        private readonly IMediator _mediator;

        public CurrentUserProvider(IActionContextAccessor contextAccessor, IMediator mediator)
        {
            _context = contextAccessor.ActionContext;
            _mediator = mediator;
        }

        public async Task<User> GetCurrentUser()
        {
            if (_context.HttpContext.User.HasClaim(x => x.Type == "Id"))
            {
                int.TryParse(_context.HttpContext.User.Claims.First(x => x.Type == "Id").Value, out int userId);
                var user = await _mediator.Send(new GetUserByIdQuery(userId));
                return user;
            }
            return null;
        }

        public bool Can(UserRights userRight)
        {
            return _context.HttpContext.User.HasClaim(ClaimTypes.Role, userRight.ToString());
        }
    }
}