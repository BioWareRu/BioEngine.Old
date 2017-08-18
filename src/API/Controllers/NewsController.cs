using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Models;
using BioEngine.API.Models.News;
using BioEngine.Common.Models;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    public class NewsController : RestController<News, int>
    {
        public NewsController(RestContext<NewsController> context) : base(context)
        {
        }

        [HttpPost]
        [UserRightsAuthorize(UserRights.AddNews)]
        public async Task<IActionResult> Post([FromBody] NewsFormModel model, [FromServices] IMapper mapper)
        {
            var command = new CreateNewsCommand(CurrentUser);
            mapper.Map(model, command);
            var newsId = await Mediator.Send(command);
            return Created(await GetNewsById(newsId));
        }

        [HttpPut("{id}")]
        [UserRightsAuthorize(UserRights.AddNews)]
        public async Task<IActionResult> Put(int id, [FromBody] NewsFormModel model, [FromServices] IMapper mapper)
        {
            var news = await GetNewsById(id);
            if (news == null)
            {
                return NotFound();
            }

            if (!HasRights(UserRights.EditNews) && news.AuthorId != CurrentUser.Id)
            {
                return Forbid();
            }

            var updateCommand = new UpdateNewsCommand(news);
            mapper.Map(model, updateCommand);
            await Mediator.Send(updateCommand);
            return Updated(await GetNewsById(id));
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
            var news = await GetNewsById(id);
            return Model(news);
        }

        [HttpDelete]
        [UserRightsAuthorize(UserRights.FullNews)]
        public override async Task<IActionResult> Delete(int id)
        {
            var news = await GetNewsById(id);

            if (news == null)
            {
                return NotFound();
            }

            var result = await Mediator.Send(new DeleteNewsCommand(news));

            if (result)
            {
                return Deleted();
            }
            return Errors(StatusCodes.Status500InternalServerError,
                new List<IErrorInterface> {new RestError("Can't delete news")});
        }

        private async Task<News> GetNewsById(int id)
        {
            return await Mediator.Send(new GetNewsByIdQuery(id));
        }

        [HttpPut("{id}/publish")]
        [UserRightsAuthorize(UserRights.PubNews)]
        public async Task<IActionResult> Publish(int id)
        {
            var news = await GetNewsById(id);

            if (news == null)
            {
                return NotFound();
            }

            if (news.Pub == 1)
            {
                return BadRequest();
            }

            await Mediator.Send(new PublishNewsCommand(news));

            return Ok();
        }

        [HttpPut("{id}/unpublish")]
        [UserRightsAuthorize(UserRights.PubNews)]
        public async Task<IActionResult> UnPublish(int id)
        {
            var news = await GetNewsById(id);

            if (news == null)
            {
                return NotFound();
            }

            if (news.Pub == 0)
            {
                return BadRequest();
            }

            await Mediator.Send(new UnPublishNewsCommand(news));

            return Ok();
        }
    }
}