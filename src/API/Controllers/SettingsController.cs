using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
// ReSharper disable once RedundantUsingDirective
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.API.Controllers
{
    public class SettingsController : RestController<APISettings, int>
    {
        public SettingsController(RestContext<SettingsController> context) : base(context)
        {
        }

        [HttpGet]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var apiSettings = HttpContext.RequestServices.GetService<IOptions<APISettings>>().Value;
            apiSettings.UserToken = HttpContext.Features.Get<ICurrentUserFeature>().Token;
            return Ok(apiSettings);
        }

        [HttpGet("{id}")]
        public override Task<IActionResult> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpDelete]
        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}