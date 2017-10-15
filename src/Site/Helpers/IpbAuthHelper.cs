using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using BioEngine.Common.Ipb;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BioEngine.Site.Helpers
{
    public static class IpbAuthHelper
    {
        private static bool GetClaims(string response, ClaimsIdentity identity, string claimsIssuer,
            ILogger ipbLogger)
        {
            var userInfo = new IpbUserInfo(response, ipbLogger);
            if (!userInfo.IsParsed) return false;
            InsertClaims(userInfo, identity, claimsIssuer);
            return true;
        }

        public static void AddIpbOauthAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
            {
                o.LoginPath = new PathString("/login");
                o.ExpireTimeSpan = TimeSpan.FromDays(30);
            }).AddOAuth("IPB",
                options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ClientId = configuration["IPB_OAUTH_CLIENT_ID"];
                    options.ClientSecret = configuration["IPB_OAUTH_CLIENT_SECRET"];
                    options.CallbackPath = new PathString(configuration["Data:OAuth:CallbackPath"]);
                    options.AuthorizationEndpoint = configuration["Data:OAuth:AuthorizationEndpoint"];
                    options.TokenEndpoint = configuration["Data:OAuth:TokenEndpoint"];
                    options.UserInformationEndpoint = configuration["Data:OAuth:UserInformationEndpoint"];
                    options.SaveTokens = true;
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            // Get the IPB user
                            var request = new HttpRequestMessage(HttpMethod.Get,
                                context.Options.UserInformationEndpoint);
                            request.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response =
                                await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var success = GetClaims(await response.Content.ReadAsStringAsync(), context.Identity,
                                context.Options.ClaimsIssuer,
                                context.HttpContext.RequestServices.GetService<ILogger<IPBApiHelper>>());
                            if (!success)
                                throw new Exception("Can't parse ipb response");
                        }
                    };
                });
        }

        private static void InsertClaims(IpbUserInfo userInfo, ClaimsIdentity identity, string claimsIssuer)
        {
            if (!string.IsNullOrEmpty(userInfo.Id))
                identity.AddClaim(
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id, ClaimValueTypes.String, claimsIssuer));

            if (!string.IsNullOrEmpty(userInfo.UserName))
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userInfo.UserName,
                    ClaimValueTypes.String,
                    claimsIssuer));

            if (!string.IsNullOrEmpty(userInfo.ProfileUrl))
                identity.AddClaim(new Claim(ClaimTypes.Webpage, userInfo.ProfileUrl, ClaimValueTypes.String,
                    claimsIssuer));

            if (!string.IsNullOrEmpty(userInfo.AvatarUrl))
                identity.AddClaim(new Claim("avatarUrl", userInfo.AvatarUrl, ClaimValueTypes.String, claimsIssuer));
        }
    }
}