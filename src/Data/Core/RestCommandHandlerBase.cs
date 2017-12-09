using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class RestCommandHandlerBase<TCommand, TResponse> : QueryHandlerBase<TCommand, TResponse>
        where TCommand : CreateCommand<TResponse>
    {
        private readonly IValidator<TCommand>[] _validators;

        protected RestCommandHandlerBase(HandlerContext context, IValidator<TCommand>[] validators) :
            base(context)
        {
            _validators = validators;
        }

        protected override async Task<TResponse> HandleCore(TCommand command)
        {
            Logger.LogInformation($"Validate command {typeof(TCommand)}");
            await Validate(command);
            Logger.LogInformation("Command is valid. Run it!");
            return await ExecuteCommandAsync(command);
        }

        private async Task Validate(TCommand command)
        {
            var validattionResuls = new List<ValidationResult>();
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(command);
                validattionResuls.Add(result);
            }
            if (validattionResuls.Any(x => !x.IsValid))
            {
                throw new ValidationException(validattionResuls.SelectMany(x => x.Errors));
            }
        }

        protected override Task<TResponse> RunQueryAsync(TCommand command)
        {
            throw new System.NotImplementedException();
        }

        protected abstract Task<TResponse> ExecuteCommandAsync(TCommand command);
    }
}