using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Common.DB;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class
        CommandWithReponseHandlerBase<TCommand, TResponse> : IAsyncRequestHandler<TCommand, TResponse>
        where TCommand : CommandWithResponseBase<TResponse>
    {
        protected readonly IMediator Mediator;
        protected readonly BWContext DBContext;
        protected readonly ILogger<CommandWithReponseHandlerBase<TCommand, TResponse>> Logger;
        protected readonly IMapper Mapper;
        private readonly IValidator<TCommand>[] _validators;


        public CommandWithReponseHandlerBase(IMediator mediator, BWContext dbContext,
            ILogger<CommandWithReponseHandlerBase<TCommand, TResponse>> logger, IValidator<TCommand>[] validators,
            IMapper mapper)
        {
            Mediator = mediator;
            DBContext = dbContext;
            Logger = logger;
            _validators = validators;
            Mapper = mapper;
        }

        public async Task<TResponse> Handle(TCommand command)
        {
            return await ExecuteCommand(command);
        }

        protected async Task Validate(TCommand command)
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

        protected abstract Task<TResponse> ExecuteCommand(TCommand command);
    }
}