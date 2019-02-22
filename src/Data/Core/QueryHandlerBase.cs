using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Data.DB;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class QueryHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected abstract Task<TResponse> RunQueryAsync(TRequest message);

        protected readonly BWContext DBContext;
        protected readonly ILogger Logger;
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;
        protected readonly BioRepository Repository;

        protected QueryHandlerBase(HandlerContext context)
        {
            DBContext = context.DBContext;
            Logger = context.Logger;
            Mediator = context.Mediator;
            Mapper = context.Mapper;
            Repository = context.Repository;
        }

        protected IQueryable<T> ApplyParentCondition<T>(IQueryable<T> query, IParentModel parent) where T : IChildModel
        {
            switch (parent.Type)
            {
                case ParentType.Game:
                    query = query.Where(x => x.GameId == (int) parent.GetId());
                    break;
                case ParentType.Developer:
                    query = query.Where(x => x.DeveloperId == (int) parent.GetId());
                    break;
                case ParentType.Topic:
                    query = query.Where(x => x.TopicId == (int) parent.GetId());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return query;
        }

        public virtual Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Run query {GetType().FullName} for message {request.GetType().FullName}");
            var result = RunQueryAsync(request);
            Logger.LogInformation("Query completed");
            return result;
        }
    }
}