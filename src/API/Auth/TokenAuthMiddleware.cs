using System.Text.Encodings.Web;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.API.Auth
{
    public class TokenAuthMiddleware : AuthenticationMiddleware<TokenAuthOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly BWContext _dbContext;
        private readonly IConfigurationRoot _configuration;

        public TokenAuthMiddleware(RequestDelegate next, IOptions<TokenAuthOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder, BWContext dbContext,
            IConfigurationRoot configuration) : base(next, options, loggerFactory,
            encoder)
        {
            _loggerFactory = loggerFactory;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        protected override AuthenticationHandler<TokenAuthOptions> CreateHandler()
        {
            return new TokenAuthenticationHandler(_dbContext, _configuration, _loggerFactory
                .CreateLogger<TokenAuthenticationHandler>());
        }
    }
}