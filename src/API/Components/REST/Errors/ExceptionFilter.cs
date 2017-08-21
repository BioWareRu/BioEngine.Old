using System.Collections.Generic;
using BioEngine.API.Components.REST.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BioEngine.API.Components.REST.Errors
{
    [UsedImplicitly]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result =
                new ObjectResult(new RestResult(StatusCodes.Status500InternalServerError,
                    new List<IErrorInterface> {new Models.RestError("Внутренняя ошибка сервера")}))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
        }
    }
}