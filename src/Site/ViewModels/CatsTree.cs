using System.Collections.Generic;

namespace BioEngine.Site.ViewModels
{
    public struct CatsTree<TCat, TEntity>
    {
        public TCat Cat { get; }
        public IReadOnlyCollection<TEntity> LastEntities { get; }

        public List<CatsTree<TCat, TEntity>> Children { get; }

        public CatsTree(TCat cat, List<TEntity> lastEntities, List<CatsTree<TCat, TEntity>> children = null)
        {
            Cat = cat;
            LastEntities = lastEntities.AsReadOnly();
            Children = children;
        }
    }
}