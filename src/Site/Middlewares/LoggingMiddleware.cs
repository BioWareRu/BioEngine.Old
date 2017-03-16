using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Site.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace BioEngine.Site.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;


        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var logProperties = new List<ILogEventEnricher>();
            if (context.Request.Headers.ContainsKey("referer"))
            {
                logProperties.Add(new PropertyEnricher("Referer", context.Request.Headers["referer"].ToString()));
            }
            if (context.Request.Headers.ContainsKey("user-agent"))
            {
                logProperties.Add(new PropertyEnricher("UserAgent",
                    context.Request.Headers["user-agent"].ToString()));
            }
            logProperties.Add(new PropertyEnricher("FullUrl", context.Request.AbsoluteUrl()));
            using (LogContext.PushProperties(logProperties.ToArray()))
            {
                await _next.Invoke(context);
            }
        }
    }
}
