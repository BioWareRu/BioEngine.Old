using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Gallery.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    public class GalleryCatsController : RestController<GalleryCat, int>
    {
        public GalleryCatsController(RestContext<GalleryCatsController> context) : base(context)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Gallery)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result =
                await Mediator.Send(new GetGalleryCategoriesQuery {LoadChildren = false, LoadLastItems = null}
                    .SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Gallery)]
        public override async Task<IActionResult> Get(int id)
        {
            var galleryCat = await GetGalleryCatById(id);
            return Model(galleryCat);
        }

        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
        
        private async Task<GalleryCat> GetGalleryCatById(int id)
        {
            return await Mediator.Send(new GetGalleryCategoryByIdQuery(id));
        }
    }
}