using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Site.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BioEngine.Site.ViewComponents
{
    public class PatreonWidgetViewComponent : ViewComponent
    {
        private readonly PatreonApiHelper _patreonApiHelper;
        private readonly ILogger<PatreonApiHelper> _logger;

        public PatreonWidgetViewComponent(PatreonApiHelper patreonApiHelper, ILogger<PatreonApiHelper> logger)
        {
            _patreonApiHelper = patreonApiHelper;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            PatreonGoal currentGoal;
            try
            {
                var goals = await _patreonApiHelper.GetGoals();
                currentGoal = goals.Where(x => x.CompletedPercentage < 100)
                    .OrderByDescending(x => x.CompletedPercentage).First();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while loading patreon goals: {ex.Message}");
                currentGoal = null;
            }
            return View(currentGoal);
        }
    }
}