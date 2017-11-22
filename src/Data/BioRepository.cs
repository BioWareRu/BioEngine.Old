using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data
{
    public class BioRepository
    {
        public GalleryRepository Gallery { get; }

        public BioRepository(GalleryRepository gallery)
        {
            Gallery = gallery;
        }
    }

    public interface IBaseBioRepository
    {
    }

    public abstract class BaseBioRepository<TEntity, TPkType> : IBaseBioRepository where TEntity : BaseModel<TPkType>
    {
        protected readonly BWContext Context;
        protected readonly ParentEntityProvider ParentEntityProvider;

        protected BaseBioRepository(BWContext context, ParentEntityProvider parentEntityProvider)
        {
            Context = context;
            ParentEntityProvider = parentEntityProvider;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            Context.Add(entity);

            await Context.SaveChangesAsync();
            return await GetById(entity.Id);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return await GetById(entity.Id);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            Context.Remove(entity);
            try
            {
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public abstract Task<TEntity> GetById(TPkType id);

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

        protected static async Task<(IEnumerable<TResponse> models, int totalCount)> GetDataAsync<TResponse>(
            IQueryable<TResponse> query,
            ModelsListQueryOptions<TResponse> message)
        {
            int? totalCount = null;
            if (message.OrderByFunc != null)
            {
                query = message.OrderByFunc(query);
            }
            if (message.Page != null && message.Page > 0)
            {
                totalCount = await query.CountAsync();
                query = query.ApplyPaging(message.PageOffset, message.PageSize);
            }
            var models = await query.ToListAsync();
            return (models, totalCount ?? models.Count);
        }
    }

    public abstract class CatBioRepository<TCat, TEntity, TPkType> : BaseBioRepository<TEntity, TPkType>
        where TCat : class, ICat<TCat, TEntity> where TEntity : BaseModel<TPkType>
    {
        protected CatBioRepository(BWContext context, ParentEntityProvider parentEntityProvider) : base(context,
            parentEntityProvider)
        {
        }

        public async Task<TCat> CreateCat(TCat cat)
        {
            Context.Add(cat);
            await Context.SaveChangesAsync();
            return await GetCatById(cat.Id);
        }

        public async Task<TCat> UpdateCat(TCat cat)
        {
            Context.Update(cat);
            await Context.SaveChangesAsync();
            return await GetCatById(cat.Id);
        }

        public async Task<bool> DeleteCat(TCat cat)
        {
            Context.Remove(cat);
            try
            {
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public abstract Task<TCat> GetCatById(int id);

        protected async Task<bool> ProccessCatAsync(TCat cat, CatsListQueryOptions<TCat> options)
        {
            if (cat.Parent == null)
            {
                cat.Parent = options.Parent ?? await ParentEntityProvider.GetModelParentAsync(cat);
            }
            if (options.LoadLastItems != null)
            {
                cat.Items = await GetCatItemsAsync(cat, options);
            }
            if (options.LoadChildren)
            {
                await Context.Entry(cat).Collection(x => x.Children).LoadAsync();
                foreach (var child in cat.Children)
                {
                    await ProccessCatAsync(child, options);
                }
            }

            return true;
        }

        protected abstract Task<IEnumerable<TEntity>> GetCatItemsAsync(TCat cat, CatsListQueryOptions<TCat> options);
    }

    public class ModelsListQueryOptions<TEntity>
    {
        public virtual int? Page { get; set; }
        public virtual int PageSize { get; set; } = 20;

        public int PageOffset => (Page - 1) * PageSize ?? 0;

        public virtual Func<IQueryable<TEntity>, IQueryable<TEntity>> OrderByFunc { get; protected set; }

        public ModelsListQueryOptions<TEntity> SetOrderBy(Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy)
        {
            OrderByFunc = orderBy;
            return this;
        }
    }
    
    public class CatsListQueryOptions<TCat> : ModelsListQueryOptions<TCat>
    {
        public IParentModel Parent { get; set; }
        public TCat ParentCat { get; set; }
        public bool OnlyRoot { get; set; }
        public string Url { get; set; }
        public bool LoadChildren { get; set; }
        public int? LoadLastItems { get; set; }
    }
}