using System.Text.Encodings.Web;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.API.Auth
{
    public class TokenAuthMiddleware : AuthenticationMiddleware<TokenAuthOptions>
    {
        private readonly TokenAuthOptions _options;
        private readonly BWContext _dbContext;

        public TokenAuthMiddleware(RequestDelegate next, IOptions<TokenAuthOptions> options, ILoggerFactory loggerFactory,
            UrlEncoder encoder, BWContext dbContext) : base(next, options, loggerFactory, encoder)
        {
            _options = options.Value;
            _dbContext = dbContext;
        }

        protected override AuthenticationHandler<TokenAuthOptions> CreateHandler()
        {
            return new TokenAuthenticationHandler(_dbContext, _options);
        }
    }
}