using System.Threading.Tasks;
using BioEngine.Common.DB;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class CommandHandlerBase<TRequest> : HandlerBase, IAsyncNotificationHandler<TRequest>
        where TRequest : INotification
    {
        protected abstract Task ExecuteCommand(TRequest command);

        protected CommandHandlerBase(IMediator mediator, BWContext dbContext, ILogger logger) : base(mediator,
            dbContext, logger)
        {
        }

        public Task Handle(TRequest command)
        {
            Logger.LogInformation($"Run command {GetType().FullName} for request {command.GetType().FullName}");
            var result = ExecuteCommand(command);
            Logger.LogInformation("Command excuted");
            return result;
        }
    }
}