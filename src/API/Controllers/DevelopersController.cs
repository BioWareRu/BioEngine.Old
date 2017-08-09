using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    public class DevelopersController : RestController<Developer, int>
    {
        public DevelopersController(IMediator mediator, CurrentUserProvider currentUserProvider) : base(mediator,
            currentUserProvider)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Developers)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result = await Mediator.Send(new GetDevelopersQuery().SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Developers)]
        public override async Task<IActionResult> Get(int id)
        {
            var developer = await Mediator.Send(new GetDeveloperByIdQuery(id));
            return Model(developer);
        }

        [HttpDelete]
        [UserRightsAuthorize(UserRights.EditDevelopers)]
        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}