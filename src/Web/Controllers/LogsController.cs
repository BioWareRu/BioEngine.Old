using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;

namespace BioEngine.Web.Controllers
{
    [Route("logs")]
    public class LogsController : Controller
    {
        private readonly LoggingLevelSwitch _switch;
        private readonly AdminAccessConfig _adminAccessConfig;

        public LogsController(LogLevelController levelController, IOptions<AdminAccessConfig> adminAccessConfig)
        {
            _switch = levelController.Switch;
            _adminAccessConfig = adminAccessConfig.Value;
        }

        [HttpGet("debug")]
        public IActionResult Debug(string accessToken)
        {
            if (accessToken != _adminAccessConfig.AdminAccessToken)
            {
                return Forbid();
            }
            _switch.MinimumLevel = LogEventLevel.Debug;
            return Ok();
        }

        [HttpGet("info")]
        public IActionResult Info(string accessToken)
        {
            if (accessToken != _adminAccessConfig.AdminAccessToken)
            {
                return Forbid();
            }
            _switch.MinimumLevel = LogEventLevel.Information;
            return Ok();
        }

        [HttpGet("error")]
        public IActionResult Error(string accessToken)
        {
            if (accessToken != _adminAccessConfig.AdminAccessToken)
            {
                return Forbid();
            }
            _switch.MinimumLevel = LogEventLevel.Error;
            return Ok();
        }
    }
}