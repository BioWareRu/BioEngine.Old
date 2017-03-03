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
        public NewsController(IJsonApiContext jsonApiContext, IEntityRepository<News, int> entityRepository,
            ILoggerFactory loggerFactory) : base(jsonApiContext, entityRepository, loggerFactory)
        {
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
    }
}