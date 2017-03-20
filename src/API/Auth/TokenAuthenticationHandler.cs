using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Auth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthOptions>
    {
        private readonly BWContext _dbContext;
        private readonly TokenAuthOptions _options;
        private readonly ILogger<TokenAuthenticationHandler> _logger;

        public TokenAuthenticationHandler(BWContext dbContext, TokenAuthOptions options, ILogger<TokenAuthenticationHandler> logger)
        {
            _dbContext = dbContext;
            _options = options;
            _logger = logger;
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

                    var token =
                        await _dbContext.AccessTokens.Where(
                                x => x.Token == tokenString && x.Expires > DateTime.Now && x.ClientId == _options.ClientId)
                            .Include(x => x.User)
                            .ThenInclude(x => x.SiteTeamMember)
                            .FirstOrDefaultAsync();
                    if (token != null)
                    {
                        var identity = new ClaimsIdentity("tokenAuth");
                        identity.AddClaim(new Claim(ClaimTypes.Name, token.User.Name));
                        foreach (UserRights userRight in Enum.GetValues(typeof(UserRights)))
                        {
                            if (token.User.HasRight(userRight, token.User.SiteTeamMember))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, userRight.ToString()));
                            }
                        }
                        var userTicket = new AuthenticationTicket(new ClaimsPrincipal(identity), null, "tokenAuth");
                        result = AuthenticateResult.Success(userTicket);
                    }
                    else
                    {
                        result = AuthenticateResult.Fail("Bad token");
                    }
                }
            }
            stopwatch.Stop();
            _logger.LogWarning($"Auth process: {stopwatch.ElapsedMilliseconds}");
            return result;
        }
    }
}