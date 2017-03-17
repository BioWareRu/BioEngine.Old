using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BioEngine.Site.Helpers
{
    public static class IpbAuthHelper
    {
        public static void ParseIpbResponse(string response, ClaimsIdentity identity, string claimsIssuer)
        {
            var user = JObject.Parse(response);

            var identifier = user.Value<string>("id");
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(
                    ClaimTypes.NameIdentifier, identifier,
                    ClaimValueTypes.String, claimsIssuer));
            }

            var userName = user.Value<string>("displayName");
            if (!string.IsNullOrEmpty(userName))
            {
                identity.AddClaim(new Claim(
                    ClaimsIdentity.DefaultNameClaimType, userName,
                    ClaimValueTypes.String, claimsIssuer));
            }

            var profileUrl = user.Value<string>("profileUrl");
            if (!string.IsNullOrEmpty(profileUrl))
            {
                identity.AddClaim(new Claim(
                    ClaimTypes.Webpage, profileUrl,
                    ClaimValueTypes.String, claimsIssuer));
            }

            var link = user.Value<string>("avatar");
            if (!string.IsNullOrEmpty(link))
            {
                identity.AddClaim(new Claim(
                    "avatarUrl", link,
                    ClaimValueTypes.String, claimsIssuer));
            }
        }

        public static void UseIpbOAuthAuthentication(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            app.UseOAuthAuthentication(new OAuthOptions()
            {
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                AuthenticationScheme = "IPB",
                DisplayName = "IPB",
                ClientId = configuration["IPB_OAUTH_CLIENT_ID"],
                ClientSecret = configuration["IPB_OAUTH_CLIENT_SECRET"],
                CallbackPath = new PathString(configuration["Data:OAuth:CallbackPath"]),
                AuthorizationEndpoint = configuration["Data:OAuth:AuthorizationEndpoint"],
                TokenEndpoint = configuration["Data:OAuth:TokenEndpoint"],
                UserInformationEndpoint = configuration["Data:OAuth:UserInformationEndpoint"],
                SaveTokens = true,
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        // Get the IPB user
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        ParseIpbResponse(await response.Content.ReadAsStringAsync(), context.Identity, context.Options.ClaimsIssuer);
                    }
                }
            });
        }
    }
}