using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using MediatR;

namespace BioEngine.API.Controllers
{
    public class TopicsController : RestController<Topic, int>
    {
        public TopicsController(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<Topic> GetItem(int id)
        {
            return await Mediator.Send(new GetTopicByIdQuery(id));
        }

        protected override async Task<(IEnumerable<Topic> items, int itemsCount)> GetItems(QueryParams queryParams)
        {
            return await Mediator.Send(new GetTopicsQuery().SetQueryParams(queryParams));
        }

        protected override Task<Topic> UpdateItem(int id, Topic model)
        {
            throw new NotImplementedException();
        }

        protected override Task<Topic> CreateItem(Topic model)
        {
            throw new NotImplementedException();
        }

        protected override Task<Topic> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }
    }
}