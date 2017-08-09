using System;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
// ReSharper disable once RedundantUsingDirective
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.API.Auth
{
    /*public class UserRightsAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly UserRights _userRights;

        public UserRightsAuthorizeAttribute(UserRights userRights)
        {
            _userRights = userRights;
        }
    }*/

    public class UserRightsAuthorizeAttribute : TypeFilterAttribute
    {
        public UserRightsAuthorizeAttribute(params UserRights[] userRights)
            : base(typeof(UserRightsCheckFilter))
        {
            Arguments = new object[] {userRights};
            Order = int.MaxValue;
        }
    }

    public class UserRightsCheckFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserRights[] _userRights;

        public UserRightsCheckFilter(UserRights[] userRights)
        {
            _userRights = userRights;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var ok = true;
            var user = await context.HttpContext.RequestServices.GetService<CurrentUserProvider>().GetCurrentUser();
            foreach (var userRight in _userRights)
            {
                ok = user.HasRight(userRight);
                if (!ok) break;
            }

            if (!ok) context.Result = new ForbidResult();
        }
    }
}