using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace BioEngine.Site.Controllers
{
    public class UserController : BaseController
    {
        public UserController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        [HttpGet("/login")]
        public async Task Login([FromServices] IHostingEnvironment env)
        {
            if (env.IsProduction())
            {
                //with ssl termination on cloudflare, AuthenticationHandler will build redirectUrl with http scheme.
                HttpContext.Request.Scheme = "https";
            }
            await HttpContext.Authentication.ChallengeAsync("IPB", new AuthenticationProperties() { RedirectUri = "/" });
        }

        [HttpGet("/logout")]
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Redirect("/");
        }
    }
}