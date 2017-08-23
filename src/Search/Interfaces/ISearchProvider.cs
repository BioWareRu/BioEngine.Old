using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Search.Interfaces
{
    public interface ISearchProvider
    {
        Task DeleteIndexAsync();
        Task<long> CountAsync(string term);
    }

    public interface ISearchProvider<T> : ISearchProvider where T : IBaseModel
    {
        Task<IEnumerable<T>> SearchAsync(string term, int limit);
        Task AddOrUpdateEntityAsync(T entitity);
        Task AddOrUpdateEntitiesAsync(IEnumerable<T> entities);
        Task DeleteEntityAsync(T entity);
    }
}