using BioEngine.API.Components.REST.Models;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BioEngine.API.Components.REST
{
    [UsedImplicitly]
    public class ValidationExceptionsFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var validationException = context.Exception as ValidationException;
            if (validationException != null)
            {
                context.Result =
                    new ObjectResult(new ValidationResultModel(validationException.Errors))
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    };
            }
        }
    }
}