using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Auth
{
    public class TokenAuthMiddleware : AuthenticationMiddleware<TokenAuthOptions>
    {
        private readonly BWContext _dbContext;

        public TokenAuthMiddleware(RequestDelegate next, IOptions<TokenAuthOptions> options, ILoggerFactory loggerFactory,
            UrlEncoder encoder, BWContext dbContext) : base(next, options, loggerFactory, encoder)
        {
            _dbContext = dbContext;
        }

        protected override AuthenticationHandler<TokenAuthOptions> CreateHandler()
        {
            return new TokenAuthenticationHandler(_dbContext);
        }
    }

    public class TokenAuthOptions : AuthenticationOptions
    {
    }
}