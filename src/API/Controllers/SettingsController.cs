using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Models;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(new APISettings(Configuration, HttpContext.Features.Get<ICurrentUserFeature>().Token));
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