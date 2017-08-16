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
    public class GamesController : RestController<Game, int>
    {
        public GamesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Games)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result = await Mediator.Send(new GetGamesQuery().SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Games)]
        public override async Task<IActionResult> Get(int id)
        {
            var game = await Mediator.Send(new GetGameByIdQuery(id));
            return Model(game);
        }

        [HttpDelete]
        [UserRightsAuthorize(UserRights.EditGames)]
        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}