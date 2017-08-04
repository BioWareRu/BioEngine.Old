using System.Threading.Tasks;
using BioEngine.Common.DB;
using MediatR;

namespace BioEngine.Data.Core
{
    internal abstract class CommandHandlerBase<TRequest> : HandlerBase, IAsyncNotificationHandler<TRequest>
        where TRequest : INotification
    {
        public abstract Task Handle(TRequest command);

        protected CommandHandlerBase(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }
    }
}