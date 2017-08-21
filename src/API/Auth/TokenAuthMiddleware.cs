using System.Text.Encodings.Web;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.API.Auth
{
    [UsedImplicitly]
    public class TokenAuthMiddleware : AuthenticationMiddleware<TokenAuthOptions>
    {
        private readonly IOptions<TokenAuthOptions> _options;
        private readonly ILoggerFactory _loggerFactory;

        public TokenAuthMiddleware(RequestDelegate next, IOptions<TokenAuthOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder, IMediator mediator) : base(next, options, loggerFactory,
            encoder)
        {
            _options = options;
            _loggerFactory = loggerFactory;
        }

        protected override AuthenticationHandler<TokenAuthOptions> CreateHandler()
        {
            return new TokenAuthenticationHandler(_loggerFactory
                .CreateLogger<TokenAuthenticationHandler>(), _options);
        }
    }
}