using System.Text.Encodings.Web;
using BioEngine.Common.DB;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Components.Ipb
{
    [UsedImplicitly]
    public class IpbAuthenticationMiddleware : AuthenticationMiddleware<IpbAuthenticationOptions>
    {
        private readonly BWContext _dbContext;

        public IpbAuthenticationMiddleware(RequestDelegate next, IOptions<IpbAuthenticationOptions> options,
            ILoggerFactory loggerFactory, UrlEncoder encoder, BWContext dbContext) : base(next, options, loggerFactory, encoder)
        {
            _dbContext = dbContext;
        }

        protected override AuthenticationHandler<IpbAuthenticationOptions> CreateHandler()
        {
            return new IpbAuthenticationHandler(_dbContext);
        }
    }

    public class IpbAuthenticationOptions : AuthenticationOptions
    {
        public string ForumUrl { get; set; }
    }
}