using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Models;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    [Route("api/[controller]")]
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

        /*protected override Task<News> UpdateItem(int id, News model)
        {
            throw new System.NotImplementedException();
        }*/


        /*protected override Task<News> DeleteItem(int id)
        {
            throw new System.NotImplementedException();
        }*/

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateNewsCommand command)
        {
            try
            {
                command.AuthorId = int.Parse(HttpContext.User.Claims.First(x => x.Type == "Id").Value);
                var news = await CreateItem(command);
                return Ok(new SaveModelReponse<News>(StatusCodes.Status201Created, news));
            }
            catch (ValidationException e)
            {
                return BadRequest(new ValidationResultModel(e.Errors));
            }
        }
    }
}