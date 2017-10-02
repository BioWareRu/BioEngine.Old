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
    public class ArticlesCatsController : RestController<ArticleCat, int>
    {
        public ArticlesCatsController(RestContext<ArticlesCatsController> context) : base(context)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Articles)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result =
                await Mediator.Send(new GetArticlesCategoriesQuery {LoadChildren = false, LoadLastItems = null}
                    .SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Articles)]
        public override async Task<IActionResult> Get(int id)
        {
            var articleCat = await GetArticleCatById(id);
            return Model(articleCat);
        }

        [HttpPost]
        [UserRightsAuthorize(UserRights.AddArticles)]
        public async Task<IActionResult> Post([FromBody] ArticleCatFormModel model, [FromServices] IMapper mapper)
        {
            var command = new CreateArticleCatCommand();
            mapper.Map(model, command);
            var articleCatId = await Mediator.Send(command);
            return Created(await GetArticleCatById(articleCatId));
        }

        [HttpPut("{id}")]
        [UserRightsAuthorize(UserRights.EditArticles)]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleCatFormModel model,
            [FromServices] IMapper mapper)
        {
            var articleCat = await GetArticleCatById(id);
            if (articleCat == null)
            {
                return NotFound();
            }

            var updateCommand = new UpdateArticleCatCommand(articleCat);
            mapper.Map(model, updateCommand);
            await Mediator.Send(updateCommand);
            return Updated(await GetArticleCatById(id));
        }

        [HttpDelete("{id}")]
        [UserRightsAuthorize(UserRights.FullArticles)]
        public override async Task<IActionResult> Delete(int id)
        {
            var articleCat = await GetArticleCatById(id);
            if (articleCat == null)
            {
                return NotFound();
            }
            
            var result = await Mediator.Send(new DeleteArticleCatCommand(articleCat));
            if (result)
            {
                return Deleted();
            }
            return Errors(StatusCodes.Status500InternalServerError,
                new List<IErrorInterface> {new RestError("Can't delete article category")});
        }

        private async Task<ArticleCat> GetArticleCatById(int id)
        {
            return await Mediator.Send(new GetArticleCategoryByIdQuery(id));
        }
    }
}