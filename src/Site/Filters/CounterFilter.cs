using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BioEngine.Site.Filters
{
    public class CounterFilter : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<string, Summary> PathSummaries = new ConcurrentDictionary<string, Summary>();

        private readonly ILogger _logger;

        private Stopwatch _timer;

        public CounterFilter(ILogger<CounterFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            _timer.Stop();
            GetSummary(context.ActionDescriptor.DisplayName)?.Observe(_timer.ElapsedMilliseconds);
        }

        private static readonly Regex ActionNameRegex = new Regex("BioEngine_Site_Controllers_(.*)\\s\\(");

        private Summary GetSummary(string action)
        {
            var actionName = ActionNameRegex.Match(action.Replace('.', '_'));
            return actionName.Success ? PathSummaries.GetOrAdd($"{actionName.Groups[1]}",
                newPath =>
                {
                    try
                    {
                        var metric = Metrics.CreateSummary($"query_{newPath}_summary", $"Queries summary for {newPath}");
                        return metric;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Can't create metric for {newPath}: {ex.Message}");
                    }
                    return null;
                }) : null;
        }
    }
}
