﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Ipb;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.API.Auth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthOptions>
    {
        private static readonly ConcurrentDictionary<string, User> TokenUsers = new ConcurrentDictionary<string, User>()
            ;

        private readonly IConfigurationRoot _configuration;
        private readonly BWContext _dbContext;
        private readonly ILogger<TokenAuthenticationHandler> _logger;

        private readonly TokenAuthOptions _options;

        public TokenAuthenticationHandler(BWContext dbContext,
            IConfigurationRoot configuration, ILogger<TokenAuthenticationHandler> logger,
            IOptions<TokenAuthOptions> options)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_options.DevMode)
                return HandleAuthenticateDevAsync();
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
                        var user = await GetUser(tokenString);
                        if (user != null)
                        {
                            var userTicket = AuthenticationTicket(user);
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
            _logger.LogWarning($"Auth process: {stopwatch.ElapsedMilliseconds}");
            return result;
        }

        private static AuthenticationTicket AuthenticationTicket(User user)
        {
            var identity = new ClaimsIdentity("tokenAuth");
            identity.AddClaim(new Claim("Id", user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
            foreach (UserRights userRight in Enum.GetValues(typeof(UserRights)))
                if (user.HasRight(userRight, user.SiteTeamMember))
                    identity.AddClaim(new Claim(ClaimTypes.Role, userRight.ToString()));
            var userTicket =
                new AuthenticationTicket(new ClaimsPrincipal(identity), null, "tokenAuth");
            return userTicket;
        }

        private static AuthenticateResult HandleAuthenticateDevAsync()
        {
            var user = new User {Id = 1, Name = "Admin", GroupId = 4};
            var userTicket = AuthenticationTicket(user);
            return AuthenticateResult.Success(userTicket);
        }

        private async Task<User> GetUser(string token)
        {
            var exists = TokenUsers.TryGetValue(token, out var user);
            if (!exists)
            {
                var userInfo = await GetUserInformation(token);
                if (userInfo.IsParsed)
                {
                    var id = int.Parse(userInfo.Id);
                    user =
                        await _dbContext.Users.Where(
                                u => u.Id == id
                            )
                            .Include(x => x.SiteTeamMember)
                            .FirstOrDefaultAsync();
                    if (user != null)
                        TokenUsers.TryAdd(token, user);
                }
            }
            return user;
        }

        private async Task<IpbUserInfo> GetUserInformation(string token)
        {
            var userInformationEndpoint = _configuration["Data:OAuth:UserInformationEndpoint"];

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

            var userInformation = new IpbUserInfo(msg, _logger);

            return userInformation;
        }
    }
}