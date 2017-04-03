using System.Threading.Tasks;
using BioEngine.Common.Models;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "tokenAuth")]
    public class NewsController : JsonApiController<News>
    {
        private readonly IEntityRepository<News, int> _entityRepository;

        public NewsController(IJsonApiContext jsonApiContext, IEntityRepository<News, int> entityRepository,
            ILoggerFactory loggerFactory) : base(jsonApiContext, entityRepository, loggerFactory)
        {
            _entityRepository = entityRepository;
        }

        public override async Task<IActionResult> GetAsync()
        {
            if (HttpContext.User.IsInRole(UserRights.News.ToString()))
            {
                var result = await base.GetAsync();
                return result;
            }
            return new ForbidResult();
        }

        [HttpPost("{id:int}/publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var news = await _entityRepository.GetAsync(id);
            if (news != null)
            {
                news.Pub = 1;
                //await _entityRepository.UpdateAsync(news.Id, news);
                return Ok(news);
            }
            return NotFound();
        }
    }
}