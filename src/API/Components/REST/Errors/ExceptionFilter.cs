using System.Collections.Generic;
using BioEngine.API.Components.REST.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
// ReSharper disable once RedundantUsingDirective
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.API.Components.REST.Errors
{
    [UsedImplicitly]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<ExceptionFilter>>();
            var exception = context.Exception;
            logger.LogError(500, exception, $"API Exception: {exception.Message}");
            context.Result =
                new ObjectResult(new RestResult(StatusCodes.Status500InternalServerError,
                    new List<IErrorInterface> {new RestError("Внутренняя ошибка сервера")}))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
        }
    }
}