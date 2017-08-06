using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Site.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BioEngine.Site.ViewComponents
{
    public class PatreonWidgetViewComponent : ViewComponent
    {
        private readonly PatreonApiHelper _patreonApiHelper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PatreonApiHelper> _logger;

        public PatreonWidgetViewComponent(PatreonApiHelper patreonApiHelper, IMemoryCache cache,
            ILogger<PatreonApiHelper> logger)
        {
            _patreonApiHelper = patreonApiHelper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentGoal = _cache.Get<PatreonGoal>("patreonCurrentGoal");
            if (currentGoal == null)
            {
                try
                {
                    var goals = await _patreonApiHelper.GetGoals();
                    currentGoal = goals.Where(x => x.CompletedPercentage < 100)
                        .OrderByDescending(x => x.CompletedPercentage).First();
                    _cache.Set("patreonCurrentGoal", currentGoal, TimeSpan.FromHours(1));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while loading patreon goals: {ex.Message}");
                }
            }
            return View(currentGoal);
        }
    }
}