using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BioEngine.Common.Ipb;
using BioEngine.Common.Models;
using BioEngine.Data.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// ReSharper disable once RedundantUsingDirective
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.API.Auth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthOptions>
    {
        private static readonly ConcurrentDictionary<string, User> TokenUsers = new ConcurrentDictionary<string, User>()
            ;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        private IMediator GetMediator()
        {
            return Context.RequestServices.GetService<IMediator>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var result = AuthenticateResult.Fail("No token");
            //            throw new NotImplementedException();
            if (Context.Request.Headers.ContainsKey("Authorization"))
            {
                var headerString = Context.Request.Headers["Authorization"][0];
                if (headerString.Contains("Bearer"))
                {
                    var tokenString = headerString.Replace("Bearer ", "");

                    if (!string.IsNullOrEmpty(tokenString))
                    {
                        if (Options.DevMode)
                            return await HandleAuthenticateDevAsync(tokenString);
                        var user = await GetUserAsync(tokenString);
                        if (user != null)
                        {
                            var userTicket = AuthenticationTicket(user);
                            Context.Features.Set<ICurrentUserFeature>(new CurrentUserFeature(user, tokenString));
                            result = AuthenticateResult.Success(userTicket);
                        }
                        else
                        {
                            result = AuthenticateResult.Fail("Bad token");
                        }
                    }
                }
            }
            stopwatch.Stop();
            Logger.LogWarning($"Auth process: {stopwatch.ElapsedMilliseconds}");
            return result;
        }

        private static AuthenticationTicket AuthenticationTicket(User user)
        {
            var identity = new ClaimsIdentity("token");
            identity.AddClaim(new Claim("Id", user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
            foreach (UserRights userRight in Enum.GetValues(typeof(UserRights)))
                if (user.HasRight(userRight, user.SiteTeamMember))
                    identity.AddClaim(new Claim(ClaimTypes.Role, userRight.ToString()));
            var userTicket =
                new AuthenticationTicket(new ClaimsPrincipal(identity), null, "token");
            return userTicket;
        }

        private async Task<AuthenticateResult> HandleAuthenticateDevAsync(string token)
        {
            var user = await GetMediator().Send(new GetUserByIdQuery(1));
            user.AvatarUrl = "/assets/img/avatar.png";
            var userTicket = AuthenticationTicket(user);
            Context.Features.Set<ICurrentUserFeature>(new CurrentUserFeature(user, token));
            return AuthenticateResult.Success(userTicket);
        }

        private async Task<User> GetUserAsync(string token)
        {
            var exists = TokenUsers.TryGetValue(token, out var user);
            if (!exists)
            {
                var userInfo = await GetUserInformationAsync(token);
                if (userInfo.IsParsed)
                {
                    var id = int.Parse(userInfo.Id);
                    user =
                        await GetMediator().Send(new GetUserByIdQuery(id));
                    if (user != null)
                    {
                        user.AvatarUrl = userInfo.AvatarUrl;
                        TokenUsers.TryAdd(token, user);
                    }
                }
            }
            return user;
        }

        private async Task<IpbUserInfo> GetUserInformationAsync(string token)
        {
            var userInformationEndpoint = Options.UserInformationEndpointUrl;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var stringTask = client.GetStringAsync(userInformationEndpoint);

            string msg = null;
            try
            {
                msg = await stringTask;
            }
            catch (HttpRequestException e)
            {
                Logger.LogError($"Error while request user information: {e.Message}");
            }

            var userInformation = new IpbUserInfo(msg, Logger);

            return userInformation;
        }
    }

    public interface ICurrentUserFeature
    {
        User User { get; }
        string Token { get; }
    }

    public class CurrentUserFeature : ICurrentUserFeature
    {
        public User User { get; }
        public string Token { get; }

        public CurrentUserFeature(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}