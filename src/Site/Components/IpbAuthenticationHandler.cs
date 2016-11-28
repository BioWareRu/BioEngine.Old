using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace BioEngine.Site.Components
{
    public class IpbAuthenticationHandler : AuthenticationHandler<IpbAuthenticationOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            ClaimsIdentity identity;

            var userDesc = await GetIpbResponse();
            if (userDesc.MemberId != null)
            {
                identity = new ClaimsIdentity("ipb");
                identity.AddClaim(new Claim(ClaimTypes.Name, userDesc.MemberName));
                identity.AddClaim(new Claim("userId", userDesc.MemberId.ToString()));
                identity.AddClaim(new Claim("avatarUrl", userDesc.AvatarUrl));
                identity.AddClaim(new Claim("profileUrl", userDesc.ProfileUrl));
                if (userDesc.IsRenegade)
                    identity.AddClaim(new Claim("renegade", "1"));
            }
            else
            {
                identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(ClaimTypes.Name, "Guest"));
            }
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), null, "ipb");
            return AuthenticateResult.Success(ticket);
        }

        private async Task<IpbResponse> GetIpbResponse()
        {
            var baseAddress = new Uri("https://forum.bioware.ru");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            {
                using (var client = new HttpClient(handler) {BaseAddress = baseAddress})
                {
                    cookieContainer.Add(baseAddress, new Cookie("CookieName", "cookie_value"));
                    foreach (var cookie in Request.Cookies)
                    {
                        var newCookie = new Cookie(cookie.Key, cookie.Value, "/") {Secure = true};
                        cookieContainer.Add(baseAddress, newCookie);
                    }
                    var result = await client.GetAsync("/api/user.php");
                    result.EnsureSuccessStatusCode();
                    var data = await result.Content.ReadAsStringAsync();
                    var userDesc = JsonConvert.DeserializeObject<IpbResponse>(data);
                    return userDesc;
                }
            }
        }
    }

    [UsedImplicitly]
    public class IpbAuthorizationHandler : AuthorizationHandler<IpbRequestPassed>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IpbRequestPassed requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class IpbRequestPassed : IAuthorizationRequirement
    {
    }

    public struct IpbResponse
    {
        [JsonProperty("member_id")] public int? MemberId;
        [JsonProperty("member_name")] public string MemberName;
        [JsonProperty("avatarUrl")] public string AvatarUrl;
        [JsonProperty("isRenegade")] public bool IsRenegade;
        [JsonProperty("profile_url")] public string ProfileUrl;
    }
}