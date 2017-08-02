using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Site.Base;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator,
            IOptions<AppSettings> appSettingsOptions, IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
        }

        [HttpGet("/login")]
        public async Task Login([FromServices] ILogger<UserController> logger)
        {
            await HttpContext.Authentication.ChallengeAsync("IPB",
                new AuthenticationProperties() {RedirectUri = "/", IsPersistent = true});
        }

        [HttpGet("/logout")]
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Redirect("/");
        }
    }
}