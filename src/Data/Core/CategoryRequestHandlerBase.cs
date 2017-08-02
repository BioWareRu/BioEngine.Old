using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class
        CategoryRequestHandlerBase<TCat, TEntity, TRequest, TResponse> : RequestHandlerBase<TRequest, TResponse>
        where TRequest : IRequest<TResponse> where TCat : class, ICat<TCat, TEntity>
    {
        private readonly ParentEntityProvider _parentEntityProvider;

        protected CategoryRequestHandlerBase(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext)
        {
            _parentEntityProvider = parentEntityProvider;
        }

        protected async void ProcessCat(TCat cat, ICategoryRequest<TCat> message)
        {
            await DBContext.Entry(cat).Collection(x => x.Children).LoadAsync();
            cat.Parent = message.Parent ?? await _parentEntityProvider.GetModelParent(cat);
            if (message.LoadLastItems != null)
            {
                cat.Items = await GetCatItems(cat, (int) message.LoadLastItems);
            }
            if (message.LoadChildren)
            {
                foreach (var child in cat.Children)
                {
                    ProcessCat(child, message);
                }
            }
        }

        protected abstract Task<IEnumerable<TEntity>> GetCatItems(TCat cat, int count);
    }
}