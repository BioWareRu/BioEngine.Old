using BioEngine.Common.Interfaces;

namespace BioEngine.Data.Core
{
    public abstract class CategoryProcessQueryBase<TCat> : QueryBase<TCat>
        where TCat : class, ICat
    {
        public TCat Cat { get; }

        public ICategoryQuery<TCat> CategoryQuery { get; }

        protected CategoryProcessQueryBase(TCat cat, ICategoryQuery<TCat> query)
        {
            Cat = cat;
            CategoryQuery = query;
        }
    }
}