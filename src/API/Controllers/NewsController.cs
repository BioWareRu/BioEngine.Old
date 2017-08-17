using System;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Models.News;
using BioEngine.Common.Models;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
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
        public override Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<News> GetNewsById(int id)
        {
            return await Mediator.Send(new GetNewsByIdQuery(id));
        }
    }
}