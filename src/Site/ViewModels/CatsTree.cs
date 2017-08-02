using System.Collections.Generic;

namespace BioEngine.Site.ViewModels
{
    public struct CatsTree<TCat, TEntity>
    {
        public TCat Cat { get; }
        public IEnumerable<TEntity> LastEntities { get; }

        public IEnumerable<CatsTree<TCat, TEntity>> Children { get; }

        public CatsTree(TCat cat, IEnumerable<TEntity> lastEntities, IEnumerable<CatsTree<TCat, TEntity>> children = null)
        {
            Cat = cat;
            LastEntities = lastEntities;
            Children = children;
        }
    }
}