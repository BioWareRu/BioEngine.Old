using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioEngine.Common.Search
{
    public interface ISearchProvider<T> where T : ISearchModel
    {
        Task<IEnumerable<T>> Search(string term, int limit);
        Task<long> Count(string term);

        Task AddUpdateEntity(T entitity);
        Task AddUpdateEntities(IEnumerable<T> entities);
        Task DeleteEntity(T entitity);
    }
}