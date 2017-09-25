using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Files.Queries;
using Microsoft.AspNetCore.Mvc;


namespace BioEngine.API.Controllers
{
    public class FilesController : RestController<File, int>
    {
        public FilesController(RestContext<ArticlesController> context) : base(context)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Files)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result =
                await Mediator.Send(new GetFilesQuery { }.SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Files)]
        public override async Task<IActionResult> Get(int id)
        {
            var file = await GetFileById(id);
            return Model(file);
        }
        
        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
        
        private async Task<File> GetFileById(int id)
        {
            return await Mediator.Send(new GetFileByIdQuery(id));
        }
    }
}