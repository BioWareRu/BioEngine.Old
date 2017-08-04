using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Requests
{
    public class GetBlockByIdRequest : RequestBase<Block>
    {
        public GetBlockByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}