using System;
using System.Linq;
using BioEngine.API.Auth;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Errors;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    [Authorize]
    [ValidationExceptionsFilter]
    [UserExceptionFilter]
    [ExceptionFilter]
    public class UserController : Controller
    {
        [HttpGet("/v1/me")]
        public IActionResult Me()
        {
            var feature = HttpContext.Features.Get<ICurrentUserFeature>();
            var user = feature.User;
            var rights = Enum.GetValues(typeof(UserRights)).Cast<UserRights>()
                .Where(userRight => user.HasRight(userRight, user.SiteTeamMember)).ToList();
            return Ok(new {user, rights});
        }
    }
}