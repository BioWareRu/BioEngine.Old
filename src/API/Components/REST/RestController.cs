using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Auth;
using BioEngine.API.Components.REST.Errors;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BioEngine.API.Components.REST
{
    [Authorize(ActiveAuthenticationSchemes = "tokenAuth")]
    [ValidationExceptionsFilter]
    [Route("v1/[controller]")]
    public abstract class RestController<T, TPkType> : Controller where T : BaseModel<TPkType>
    {
        protected readonly IMediator Mediator;
        protected readonly IConfiguration Configuration;
        protected readonly AppSettings AppSettings;
        protected readonly ILogger Logger;

        protected RestController(RestContext context)
        {
            Mediator = context.Mediator;
            AppSettings = context.AppSettings;
            Logger = context.Logger;
            Configuration = context.Configuration;
        }

        protected User CurrentUser
        {
            get
            {
                var feature = HttpContext.Features.Get<ICurrentUserFeature>();
                return feature.User;
            }
        }

        protected bool HasRights(UserRights rights)
        {
            return CurrentUser.HasRight(rights);
        }

        public abstract Task<IActionResult> Get(QueryParams queryParams);

        public abstract Task<IActionResult> Get(TPkType id);

        public abstract Task<IActionResult> Delete(TPkType id);

        protected IActionResult Created(T model)
        {
            return SaveResponse(StatusCodes.Status201Created, model);
        }

        protected IActionResult Updated(T model)
        {
            return SaveResponse(StatusCodes.Status202Accepted, model);
        }

        protected IActionResult Deleted()
        {
            return SaveResponse(StatusCodes.Status204NoContent, null);
        }

        protected IActionResult Errors(int code, IEnumerable<IErrorInterface> errors)
        {
            return new ObjectResult(new RestResult(code, errors)) {StatusCode = code};
        }

        private IActionResult SaveResponse(int code, T model)
        {
            return new ObjectResult(new SaveModelResponse<T>(code, model)) {StatusCode = code};
        }

        protected IActionResult List((IEnumerable<T> items, int itemsCount) result)
        {
            return Ok(new ListResult<T>(result.items, result.itemsCount));
        }

        protected IActionResult Model(T model)
        {
            if (model == null)
            {
                return NotFound(new NotFoundError());
            }
            return Ok(model);
        }
    }

    public abstract class RestContext
    {
        protected RestContext(IMediator mediator, IConfiguration configuration, AppSettings appSettings, ILogger logger)
        {
            Mediator = mediator;
            Configuration = configuration;
            AppSettings = appSettings;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; }
        public ILogger Logger { get; }
    }

    public class RestContext<T> : RestContext
    {
        public RestContext(IMediator mediator, IConfigurationRoot configuration, IOptions<AppSettings> appSettings,
            ILogger<T> logger) : base(mediator, configuration, appSettings.Value,
            logger)
        {
        }
    }
}