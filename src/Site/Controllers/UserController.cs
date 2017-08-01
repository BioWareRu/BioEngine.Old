using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class UserController : BaseController
    {
        public UserController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions, IContentHelperInterface contentHelper)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions, contentHelper)
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