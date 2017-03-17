using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace BioEngine.Site.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        private static Summary ErrorsSummary = Metrics.CreateSummary("bw_errors", "BW logged errors");

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            ErrorsSummary.Observe(1);
            _logger.LogError(500, context.Exception, context.Exception.Message);
        }
    }
}