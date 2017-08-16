using System;
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

    public class UserRightsCheckFilter : Attribute, IAuthorizationFilter
    {
        private readonly UserRights[] _userRights;

        public UserRightsCheckFilter(UserRights[] userRights)
        {
            _userRights = userRights;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var ok = true;
            var user = context.HttpContext.Features.Get<ICurrentUserFeature>().User;
            foreach (var userRight in _userRights)
            {
                ok = user.HasRight(userRight);
                if (!ok) break;
            }

            if (!ok) context.Result = new ForbidResult();
        }
    }
}