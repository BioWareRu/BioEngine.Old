using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Data.Core;

namespace BioEngine.Data.Search.Queries
{
    public class SearchEntitiesQuery<T> : QueryBase<IEnumerable<T>> where T : IBaseModel
    {
        public SearchEntitiesQuery(string query, int limit = 100)
        {
            Query = query;
            Limit = limit;
        }

        public string Query { get; }
        public int Limit { get; }
    }
}