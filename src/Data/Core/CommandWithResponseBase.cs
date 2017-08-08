using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class CommandWithResponseBase<TResponse> : INotification, IRequest<TResponse>
    {
    }
}