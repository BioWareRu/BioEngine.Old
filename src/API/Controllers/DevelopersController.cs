using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using MediatR;

namespace BioEngine.API.Controllers
{
    public class DevelopersController : RestController<Developer, int>
    {
        public DevelopersController(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<Developer> GetItem(int id)
        {
            return await Mediator.Send(new GetDeveloperByIdQuery(id));
        }

        protected override async Task<(IEnumerable<Developer> items, int itemsCount)> GetItems(QueryParams queryParams)
        {
            return await Mediator.Send(new GetDevelopersQuery().SetQueryParams(queryParams));
        }

        protected override Task<Developer> UpdateItem(int id, Developer model)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Developer> CreateItem(Developer model)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Developer> DeleteItem(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}