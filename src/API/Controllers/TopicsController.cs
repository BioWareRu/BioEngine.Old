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
    public class TopicsController : RestController<Topic, int>
    {
        public TopicsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Developers)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result = await Mediator.Send(new GetTopicsQuery().SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Developers)]
        public override async Task<IActionResult> Get(int id)
        {
            var topic = await Mediator.Send(new GetTopicByIdQuery(id));
            return Model(topic);
        }

        [HttpDelete]
        [UserRightsAuthorize(UserRights.EditDevelopers)]
        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}