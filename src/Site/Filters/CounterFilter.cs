using Microsoft.AspNetCore.Mvc.Filters;
using Prometheus;
using System.Collections.Concurrent;

namespace BioEngine.Site.Filters
{
    public class CounterFilter: ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<string, Summary> PathSummaries = new ConcurrentDictionary<string, Summary>();

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
            GetSummary(context.Controller.ToString(), context.ActionDescriptor.Id);
        }

        private static Summary GetSummary(string controller, string action)
        {
            return PathSummaries.GetOrAdd($"{controller}_{action}",
                newPath => Metrics.CreateSummary($"query_{newPath}_summary", $"Queries summary for {newPath}"));
        }
    }
}
