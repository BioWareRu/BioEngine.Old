﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class QueryHandlerBase<TRequest, TResponse> : HandlerBase,
        IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected abstract Task<TResponse> RunQuery(TRequest message);

        protected QueryHandlerBase(IMediator mediator, BWContext dbContext, ILogger logger) : base(mediator, dbContext,
            logger)
        {
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

        public Task<TResponse> Handle(TRequest message)
        {
            Logger.LogInformation($"Run query {GetType().FullName} for message {message.GetType().FullName}");
            var result = RunQuery(message);
            Logger.LogInformation("Query completed");
            return result;
        }
    }
}