using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Requests
{
    public class GetMenuByKeyRequest : RequestBase<Menu>
    {
        public GetMenuByKeyRequest(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}