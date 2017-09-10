using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
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

        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        private async Task<ArticleCat> GetArticleCatById(int id)
        {
            return await Mediator.Send(new GetArticleCategoryByIdQuery(id));
        }
    }
}