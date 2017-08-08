using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.News.Queries;
using MediatR;

namespace BioEngine.API.Controllers
{
    public class NewsController : RestController<News, int>
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<News> GetItem(int id)
        {
            return await Mediator.Send(new GetNewsByIdQuery(id));
        }

        protected override async Task<(IEnumerable<News> items, int itemsCount)> GetItems(QueryParams queryParams)
        {
            return await Mediator.Send(new GetNewsQuery().SetQueryParams(queryParams));
        }

        protected override Task<News> UpdateItem(int id, News model)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<News> CreateItem(News model)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<News> DeleteItem(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}