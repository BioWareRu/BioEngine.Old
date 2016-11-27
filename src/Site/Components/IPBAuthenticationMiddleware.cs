using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BioEngine.Site.Components
{
    public class IPBAuthenticationMiddleware : AuthenticationMiddleware<IpbAuthenticationOptions>
    {
        public IPBAuthenticationMiddleware(RequestDelegate next, IOptions<IpbAuthenticationOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
            options.ToString();
        }

        protected override AuthenticationHandler<IpbAuthenticationOptions> CreateHandler()
        {
            return new IpbAuthenticationHandler();
        }
    }

    public class IpbAuthenticationOptions : AuthenticationOptions
    {
        public string ForumUrl { get; set; }
    }
}
