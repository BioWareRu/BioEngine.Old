using System.Collections.Generic;

namespace BioEngine.Common.Search
{
    public interface ISearchProvider<T> where T : ISearchModel
    {
        IEnumerable<T> Search(string term, int limit);
        long Count(string term);

        void AddUpdateEntity(T skill);
        void DeleteSkill(long updateId);
    }
}