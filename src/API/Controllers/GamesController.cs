using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using MediatR;

namespace BioEngine.API.Controllers
{
    public class GamesController : RestController<Game, int>
    {
        public GamesController(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<Game> GetItem(int id)
        {
            return await Mediator.Send(new GetGameByIdQuery(id));
        }

        protected override async Task<(IEnumerable<Game> items, int itemsCount)> GetItems(QueryParams queryParams)
        {
            return await Mediator.Send(new GetGamesQuery().SetQueryParams(queryParams));
        }

        protected override async Task<Game> UpdateItem(int id, Game model)
        {
            throw new System.NotImplementedException();
        }

        protected override async Task<Game> CreateItem(Game model)
        {
            throw new System.NotImplementedException();
        }

        protected override async Task<Game> DeleteItem(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}