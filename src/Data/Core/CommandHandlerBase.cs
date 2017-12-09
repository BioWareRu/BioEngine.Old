using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Data.DB;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class CommandHandlerBase<TRequest> : AsyncNotificationHandler<TRequest>
        where TRequest : INotification
    {
        protected abstract Task ExecuteCommandAsync(TRequest command);

        protected readonly BWContext DBContext;
        protected readonly ILogger Logger;
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;
        protected readonly BioRepository Repository;

        protected CommandHandlerBase(HandlerContext context)
        {
            DBContext = context.DBContext;
            Logger = context.Logger;
            Mediator = context.Mediator;
            Mapper = context.Mapper;
            Repository = context.Repository;
        }

        protected override Task HandleCore(TRequest command)
        {
            Logger.LogInformation($"Run command {GetType().FullName} for request {command.GetType().FullName}");
            var result = ExecuteCommandAsync(command);
            Logger.LogInformation("Command excuted");
            return result;
        }
    }
}