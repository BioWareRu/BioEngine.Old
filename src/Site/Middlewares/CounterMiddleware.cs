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
        private static readonly ConcurrentDictionary<string, Summary> PathSummaries = new ConcurrentDictionary<string, Summary>();

        private readonly RequestDelegate _next;

        private static Summary QueriesSummary = Metrics.CreateSummary("queriesSummary", "Queries stats");

        public CounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static Summary GetSummary(string path)
        {
            path = path.Replace('/', '_').Replace('.', '_').Replace('-', '_');
            return PathSummaries.GetOrAdd(path,
                newPath => Metrics.CreateSummary($"query_{newPath}_summary", $"Queries summary for {newPath}"));
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await _next.Invoke(context);
            stopWatch.Stop();

            QueriesSummary.Observe(stopWatch.ElapsedMilliseconds);
            GetSummary(context.Request.Path).Observe(stopWatch.ElapsedMilliseconds);
        }
    }
}