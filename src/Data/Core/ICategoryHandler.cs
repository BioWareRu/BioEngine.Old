using BioEngine.Common.Interfaces;

namespace BioEngine.Data.Core
{
    public interface ICategoryHandler<in TCat, TEntity> where TCat : class, ICat
    {
        void ProcessCat(TCat cat, ICategoryRequest<TCat> message);
    }
}