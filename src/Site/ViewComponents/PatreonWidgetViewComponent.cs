using System.Linq;
using System.Threading.Tasks;
using BioEngine.Site.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class PatreonWidgetViewComponent : ViewComponent
    {
        private readonly PatreonApiHelper _patreonApiHelper;

        public PatreonWidgetViewComponent(PatreonApiHelper patreonApiHelper)
        {
            _patreonApiHelper = patreonApiHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var goals = await _patreonApiHelper.GetGoals();
            var currentGoal = goals.Where(x => x.CompletedPercentage < 100)
                .OrderByDescending(x => x.CompletedPercentage).First();
            return View(currentGoal);
        }
    }
}