using System;
using System.Threading.Tasks;
using BioEngine.Content.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Controllers
{
    [Route("v1/[controller]")]
    public class PatreonController : Controller
    {
        [HttpGet("current-goal")]
        public async Task<IActionResult> CurrentGoal([FromServices] PatreonApiHelper apiHelper,
            [FromServices] IMemoryCache cache, [FromServices] ILogger<PatreonController> logger)
        {
            var currentGoal = cache.Get<PatreonGoal>("patreonCurrentGoal");
            if (currentGoal == null)
            {
                try
                {
                    currentGoal = await apiHelper.GetCurrentGoal();
                    cache.Set("patreonCurrentGoal", currentGoal, TimeSpan.FromHours(1));
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error while loading patreon goals: {ex.Message}");
                }
            }
            return Ok(currentGoal);
        }
    }
}