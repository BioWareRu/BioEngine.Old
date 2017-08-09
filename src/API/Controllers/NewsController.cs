using System;
using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Models;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : RestController<News, int>
    {
        public NewsController(IMediator mediator, CurrentUserProvider currentUserProvider) : base(mediator,
            currentUserProvider)
        {
        }

        [HttpPost]
        [UserRightsAuthorize(UserRights.AddNews)]
        public async Task<IActionResult> Post([FromBody] CreateNewsCommand command)
        {
            try
            {
                command.AuthorId = CurrentUserProvider.GetCurrentUser().Id;
                var news = await Mediator.Send(command);
                return Created(news);
            }
            catch (ValidationException e)
            {
                return BadRequest(new ValidationResultModel(e.Errors));
            }
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.News)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result = await Mediator.Send(new GetNewsQuery {WithUnPublishedNews = true}.SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.News)]
        public override async Task<IActionResult> Get(int id)
        {
            var news = await Mediator.Send(new GetNewsByIdQuery(id));
            return Model(news);
        }

        [HttpDelete]
        [UserRightsAuthorize(UserRights.FullNews)]
        public override Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}