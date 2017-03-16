using Microsoft.AspNetCore.Mvc.Filters;
using Prometheus;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BioEngine.Site.Filters
{
    public class CounterFilter : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<string, Summary> PathSummaries = new ConcurrentDictionary<string, Summary>();

        public Stopwatch timer;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            timer.Stop();
            GetSummary(context.ActionDescriptor.DisplayName).Observe(timer.ElapsedMilliseconds);
        }

        private static Summary GetSummary(string action)
        {
            return PathSummaries.GetOrAdd($"{action.Replace('.', '_')}",
                newPath => Metrics.CreateSummary($"query_{newPath}_summary", $"Queries summary for {newPath}"));
        }
    }
}
