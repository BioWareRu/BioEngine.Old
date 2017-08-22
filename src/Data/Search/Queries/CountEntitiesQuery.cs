using BioEngine.Common.Base;
using BioEngine.Data.Core;

namespace BioEngine.Data.Search.Queries
{
    public class CountEntitiesQuery<T> : QueryBase<long> where T : IBaseModel
    {
        public CountEntitiesQuery(string query)
        {
            Query = query;
        }

        public string Query { get; }
    }
}