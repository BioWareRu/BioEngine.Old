using System.Linq;
using BioEngine.API.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BioEngine.API.Components.REST.Models
{
    public class ValidationResultModel : RestResult
    {
        [UsedImplicitly]
        public string Message { get; } = "Success";

        [UsedImplicitly]
        public bool IsSuccess { get; } = true;

        public ValidationResultModel(ModelStateDictionary modelState) : base(StatusCodes.Status422UnprocessableEntity)
        {
            if (!modelState.IsValid)
            {
                IsSuccess = false;
                Message = "Validation Failed";
                Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key.ToCamelCase(), x.ErrorMessage)))
                    .ToList();
            }
        }
    }

    public class ValidationError : ErrorInterface
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}