using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Errors;
using BioEngine.API.Models.Articles;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Articles.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    public class ArticlesController : RestController<Article, int>
    {
        public ArticlesController(RestContext<ArticlesController> context) : base(context)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Articles)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result =
                await Mediator.Send(new GetArticlesQuery {WithUnPublishedArticles = true}.SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Articles)]
        public override async Task<IActionResult> Get(int id)
        {
            var article = await GetArticleById(id);
            return Model(article);
        }

        [HttpDelete("{id}")]
        [UserRightsAuthorize(UserRights.FullArticles)]
        public override async Task<IActionResult> Delete(int id)
        {
            var article = await GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            var result = await Mediator.Send(new DeleteArticleCommand(article));

            if (result)
            {
                return Deleted();
            }
            return Errors(StatusCodes.Status500InternalServerError,
                new List<IErrorInterface> {new RestError("Can't delete article")});
        }

        private async Task<Article> GetArticleById(int id)
        {
            return await Mediator.Send(new GetArticleByIdQuery(id));
        }

        [HttpPut("{id}/publish")]
        [UserRightsAuthorize(UserRights.PubArticles)]
        public async Task<IActionResult> Publish(int id)
        {
            var article = await GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            await Mediator.Send(new PublishArticleCommand(article));

            return Ok();
        }

        [HttpPut("{id}/unpublish")]
        [UserRightsAuthorize(UserRights.PubArticles)]
        public async Task<IActionResult> UnPublish(int id)
        {
            var article = await GetArticleById(id);

            if (article == null)
            {
                return NotFound();
            }

            await Mediator.Send(new UnPublishArticleCommand(article));

            return Ok();
        }

        [HttpPost]
        [UserRightsAuthorize(UserRights.AddArticles)]
        public async Task<IActionResult> Post([FromBody] ArticleFormModel model, [FromServices] IMapper mapper)
        {
            var command = new CreateArticleCommand(CurrentUser);
            mapper.Map(model, command);
            var articleId = await Mediator.Send(command);
            return Created(await GetArticleById(articleId));
        }

        [HttpPut("{id}")]
        [UserRightsAuthorize(UserRights.AddArticles)]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleFormModel model, [FromServices] IMapper mapper)
        {
            var article = await GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            if (!HasRights(UserRights.EditArticles) && article.AuthorId != CurrentUser.Id)
            {
                return Forbid();
            }

            var updateCommand = new UpdateArticleCommand(article);
            mapper.Map(model, updateCommand);
            await Mediator.Send(updateCommand);
            return Updated(await GetArticleById(id));
        }
    }
}