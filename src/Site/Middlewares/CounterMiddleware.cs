using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Prometheus;

namespace BioEngine.Site.Middlewares
{
    [UsedImplicitly]
    public class CounterMiddleware
    {
        private readonly RequestDelegate _next;

        private static Summary QueriesSummary = Metrics.CreateSummary("queriesSummary", "Queries stats");

        public CounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await _next.Invoke(context);
            stopWatch.Stop();

            QueriesSummary.Observe(stopWatch.ElapsedMilliseconds);
        }
    }
}