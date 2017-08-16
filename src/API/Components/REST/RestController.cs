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

namespace BioEngine.API.Components.REST
{
    [Authorize(ActiveAuthenticationSchemes = "tokenAuth")]
    [ValidationExceptionsFilter]
    [Route("v1/[controller]")]
    public abstract class RestController<T, TPkType> : Controller where T : BaseModel<TPkType>
    {
        protected readonly IMediator Mediator;

        protected RestController(IMediator mediator)
        {
            Mediator = mediator;
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

        private IActionResult SaveResponse(int code, T model)
        {
            return Ok(new SaveModelResponse<T>(code, model));
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
}