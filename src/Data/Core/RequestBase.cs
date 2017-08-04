using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class QueryBase<TResponse> : IRequest<TResponse>
    {
    }
}