using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest message);

        protected readonly BWContext DBContext;
        protected readonly IMediator Mediator;

        protected RequestHandlerBase(IMediator mediator, BWContext dbContext)
        {
            DBContext = dbContext;
            Mediator = mediator;
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
    }
}