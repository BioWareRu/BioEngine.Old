using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class
        CategoryProcessHandlerBase<TCat, TEntity> : RequestHandlerBase<CategoryProcessRequestBase<TCat>, TCat>
        where TCat : class, ICat<TCat, TEntity>
    {
        private readonly ParentEntityProvider _parentEntityProvider;

        protected CategoryProcessHandlerBase(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext)
        {
            _parentEntityProvider = parentEntityProvider;
        }

        public override async Task<TCat> Handle(CategoryProcessRequestBase<TCat> message)
        {
            await ProcessCat(message.Cat, message.CategoryRequest);
            return message.Cat;
        }

        private async Task<bool> ProcessCat(TCat cat, ICategoryRequest<TCat> request)
        {
            await DBContext.Entry(cat).Collection(x => x.Children).LoadAsync();
            cat.Parent = request.Parent ?? await _parentEntityProvider.GetModelParent(cat);
            if (request.LoadLastItems != null)
            {
                cat.Items = await GetCatItems(cat, (int) request.LoadLastItems);
            }
            if (request.LoadChildren)
            {
                foreach (var child in cat.Children)
                {
                    await ProcessCat(child, request);
                }
            }

            return true;
        }

        protected abstract Task<IEnumerable<TEntity>> GetCatItems(TCat cat, int count);
    }
}