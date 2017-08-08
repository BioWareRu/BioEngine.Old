using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;

namespace BioEngine.Data.Core
{
    public abstract class ModelsListQueryBase<TEntity> : QueryBase<(IEnumerable<TEntity> models, int totalCount)>
        where TEntity : IBaseModel
    {
        public virtual int? Page { get; set; }
        public virtual int PageSize { get; set; } = 20;

        public int PageOffset => (Page - 1) * PageSize ?? 0;

        public virtual Func<IQueryable<TEntity>, IQueryable<TEntity>> OrderByFunc { get; protected set; }

        public ModelsListQueryBase<TEntity> SetOrderBy(Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy)
        {
            OrderByFunc = orderBy;
            return this;
        }
    }
}