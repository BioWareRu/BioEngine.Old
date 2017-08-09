using System;
using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.News.Commands;
using BioEngine.Data.News.Queries;
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
        public async Task<IActionResult> Post([FromBody] CreateNewsCommand createCommand)
        {
            createCommand.AuthorId = CurrentUserProvider.GetCurrentUser().Id;
            var newsId = await Mediator.Send(createCommand);
            return Created(await GetNewsById(newsId));
        }

        [HttpPut("{id}")]
        [UserRightsAuthorize(UserRights.AddNews)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateNewsCommand updateCommand)
        {
            var news = await GetNewsById(id);
            if (news == null)
            {
                return NotFound();
            }

            if (!CurrentUserProvider.Can(UserRights.EditNews) && news.AuthorId != await CurrentUserProvider.GetUserId())
            {
                return Forbid();
            }

            updateCommand.SetModel(news);
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