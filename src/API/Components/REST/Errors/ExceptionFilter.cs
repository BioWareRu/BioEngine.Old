using System.Collections.Generic;
using BioEngine.API.Components.REST.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Components.REST.Errors
{
    [UsedImplicitly]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.LogError(500, exception, $"API Exception: {exception.Message}");
            context.Result =
                new ObjectResult(new RestResult(StatusCodes.Status500InternalServerError,
                    new List<IErrorInterface> {new Models.RestError("Внутренняя ошибка сервера")}))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
        }
    }
}