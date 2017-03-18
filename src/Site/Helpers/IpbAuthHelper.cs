using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BioEngine.Site.Helpers
{
    public static class IpbAuthHelper
    {
        public static bool ParseIpbResponse(string response, ClaimsIdentity identity, string claimsIssuer, ILogger ipbLogger)
        {
            ipbLogger.LogWarning($"Response from IPB: {response}");
            JObject user;
            try
            {
                user = JObject.Parse(response);
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb response: {ex.Message}");
                return false;
            }

            string identifier;
            try
            {
                identifier = user.Value<string>("id");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb id: {ex.Message}");
                return false;
            }
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, claimsIssuer));
            }

            string userName;
            try
            {
                userName = user.Value<string>("displayName");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb id: {ex.Message}");
                return false;
            }
            if (!string.IsNullOrEmpty(userName))
            {
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userName, ClaimValueTypes.String, claimsIssuer));
            }

            string profileUrl;
            try
            {
                profileUrl = user.Value<string>("profileUrl");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb profileUrl: {ex.Message}");
                return false;
            }
            if (!string.IsNullOrEmpty(profileUrl))
            {
                identity.AddClaim(new Claim(ClaimTypes.Webpage, profileUrl, ClaimValueTypes.String, claimsIssuer));
            }

            string avatarUrl;
            try
            {
                avatarUrl = user.Value<string>("avatar");
            }
            catch (Exception ex)
            {
                ipbLogger.LogError($"Error while parsing ipb avatarUrl: {ex.Message}");
                return false;
            }
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                identity.AddClaim(new Claim("avatarUrl", avatarUrl, ClaimValueTypes.String, claimsIssuer));
            }
            return true;
        }

        public static void UseIpbOAuthAuthentication(this IApplicationBuilder app, IConfigurationRoot configuration, ILogger ipbLogger)
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

                        var success = ParseIpbResponse(await response.Content.ReadAsStringAsync(), context.Identity, context.Options.ClaimsIssuer, ipbLogger);
                        if (!success)
                        {
                            throw new Exception("Can't parse ipb response");
                        }
                    }
                }
            });
        }
    }
}