using System.Collections.Generic;

namespace BioEngine.Common.Search
{
    public interface ISearchProvider<T> where T : ISearchModel
    {
        IEnumerable<T> Search(string term, int limit);
        long Count(string term);

        void AddUpdateEntity(T entitity);
        void AddUpdateEntities(IEnumerable<T> entities);
        void DeleteEntity(long updateId);
    }
}