using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BioEngine.API.Components.REST.Models
{
    public class ValidationResultModel : RestResult
    {
        [UsedImplicitly]
        public string Message { get; } = "Success";

        [UsedImplicitly]
        public bool IsSuccess { get; } = true;

        public ValidationResultModel(IEnumerable<ValidationFailure> errors) : base(StatusCodes
            .Status422UnprocessableEntity)
        {
            var validationFailures = errors as ValidationFailure[] ?? errors.ToArray();
            if (validationFailures.Any())
            {
                IsSuccess = false;
                Message = "Validation Failed";
                Errors = validationFailures
                    .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
                    .ToList();
            }
        }
    }

    public class ValidationError : IErrorInterface
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
        public ValidationFailedResult(IEnumerable<ValidationFailure> errors)
            : base(new ValidationResultModel(errors))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}