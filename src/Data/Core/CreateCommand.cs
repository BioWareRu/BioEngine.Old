using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class CreateCommand<TResponse> : INotification, IRequest<TResponse>
    {
    }
}