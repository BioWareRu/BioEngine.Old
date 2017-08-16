using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class CommandHandlerBase<TRequest> : HandlerBase, IAsyncNotificationHandler<TRequest>
        where TRequest : INotification
    {
        protected abstract Task ExecuteCommand(TRequest command);

        protected CommandHandlerBase(HandlerContext context) : base(context)
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