using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Data.DB;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class CommandHandlerBase<TRequest> : INotificationHandler<TRequest>
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
       
        public Task Handle(TRequest notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Run command {GetType().FullName} for request {notification.GetType().FullName}");
            var result = ExecuteCommandAsync(notification);
            Logger.LogInformation("Command excuted");
            return result;
        }
    }
}