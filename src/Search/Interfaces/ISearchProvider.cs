using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Search.Interfaces
{
    public interface ISearchProvider
    {
        Task DeleteIndex();
        Task<long> Count(string term);
    }

    public interface ISearchProvider<T> : ISearchProvider where T : IBaseModel
    {
        Task<IEnumerable<T>> Search(string term, int limit);
        Task AddUpdateEntity(T entitity);
        Task AddUpdateEntities(IEnumerable<T> entities);
        Task DeleteEntity(T entity);
    }
}