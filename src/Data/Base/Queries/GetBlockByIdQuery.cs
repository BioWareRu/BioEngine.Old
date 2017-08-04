using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetBlockByIdQuery : QueryBase<Block>
    {
        public GetBlockByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}