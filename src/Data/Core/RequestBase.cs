using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
    }
}