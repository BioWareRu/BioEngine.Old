using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using MediatR;

namespace BioEngine.Data.Core
{
    internal abstract class CategoryProcessHandlerBase<TRequest, TCat, TEntity> : QueryHandlerBase<TRequest, TCat>
        where TCat : class, ICat<TCat, TEntity> where TRequest : CategoryProcessQueryBase<TCat>, IRequest<TCat>
    {
        private readonly ParentEntityProvider _parentEntityProvider;

        protected CategoryProcessHandlerBase(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext)
        {
            _parentEntityProvider = parentEntityProvider;
        }

        public override async Task<TCat> Handle(TRequest message)
        {
            await ProcessCat(message.Cat, message.CategoryQuery);
            return message.Cat;
        }

        private async Task<bool> ProcessCat(TCat cat, ICategoryQuery<TCat> query)
        {
            await DBContext.Entry(cat).Collection(x => x.Children).LoadAsync();
            cat.Parent = query.Parent ?? await _parentEntityProvider.GetModelParent(cat);
            if (query.LoadLastItems != null)
            {
                cat.Items = await GetCatItems(cat, (int) query.LoadLastItems);
            }
            if (query.LoadChildren)
            {
                foreach (var child in cat.Children)
                {
                    await ProcessCat(child, query);
                }
            }

            return true;
        }

        protected abstract Task<IEnumerable<TEntity>> GetCatItems(TCat cat, int count);
    }
}