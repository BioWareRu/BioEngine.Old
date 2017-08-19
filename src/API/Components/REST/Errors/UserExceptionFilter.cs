using System.Collections.Generic;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Base;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BioEngine.API.Components.REST.Errors
{
    [UsedImplicitly]
    public class UserExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var userException = context.Exception as UserException;
            if (userException != null)
            {
                context.Result =
                    new ObjectResult(new RestResult(StatusCodes.Status500InternalServerError,
                        new List<IErrorInterface> {new Models.RestError(userException.Message)}))
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
            }
        }
    }
}