using System.Threading.Tasks;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Models.Gallery;
using BioEngine.Common.Models;
using BioEngine.Data.Gallery.Commands;
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
        
        [HttpPost]
        [UserRightsAuthorize(UserRights.AddGallery)]
        public async Task<IActionResult> Post([FromBody] GalleryCatFormModel model, [FromServices] IMapper mapper)
        {
            var command = new CreateGalleryCatCommand();
            mapper.Map(model, command);
            var galleryCatId = await Mediator.Send(command);
            return Created(await GetGalleryCatById(galleryCatId));
        }
        
        [HttpPut("{id}")]
        [UserRightsAuthorize(UserRights.EditGallery)]
        public async Task<IActionResult> Put(int id, [FromBody] GalleryCatFormModel model, [FromServices] IMapper mapper)
        {
            var galleryCat = await GetGalleryCatById(id);
            if (galleryCat == null)
            {
                return NotFound();
            }

            var updateCommand = new UpdateGalleryCatCommand(galleryCat);
            mapper.Map(model, updateCommand);
            await Mediator.Send(updateCommand);
            return Updated(await GetGalleryCatById(id));
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