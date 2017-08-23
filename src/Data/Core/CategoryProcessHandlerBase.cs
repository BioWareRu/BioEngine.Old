using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using MediatR;

namespace BioEngine.Data.Core
{
    internal abstract class CategoryProcessHandlerBase<TRequest, TCat, TEntity> : QueryHandlerBase<TRequest, TCat>
        where TCat : class, ICat<TCat, TEntity> where TRequest : CategoryProcessQueryBase<TCat>, IRequest<TCat>
    {
        private readonly ParentEntityProvider _parentEntityProvider;

        protected CategoryProcessHandlerBase(HandlerContext context, ParentEntityProvider parentEntityProvider) :
            base(context)
        {
            _parentEntityProvider = parentEntityProvider;
        }

        protected override async Task<TCat> RunQueryAsync(TRequest message)
        {
            await ProcessCatAsync(message.Cat, message.CategoryQuery);
            return message.Cat;
        }

        private async Task<bool> ProcessCatAsync(TCat cat, ICategoryQuery<TCat> query)
        {
            await DBContext.Entry(cat).Collection(x => x.Children).LoadAsync();
            cat.Parent = query.Parent ?? await _parentEntityProvider.GetModelParentAsync(cat);
            if (query.LoadLastItems != null)
            {
                cat.Items = await GetCatItemsAsync(cat, (int) query.LoadLastItems);
            }
            if (query.LoadChildren)
            {
                foreach (var child in cat.Children)
                {
                    await ProcessCatAsync(child, query);
                }
            }

            return true;
        }

        protected abstract Task<IEnumerable<TEntity>> GetCatItemsAsync(TCat cat, int count);
    }
}