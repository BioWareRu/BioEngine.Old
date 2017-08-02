using System.Threading.Tasks;
using BioEngine.Common.DB;
using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class NotificationHandlerBase<TRequest> : HandlerBase, IAsyncNotificationHandler<TRequest>
        where TRequest : INotification
    {
        public abstract Task Handle(TRequest notification);

        public NotificationHandlerBase(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }
    }
}