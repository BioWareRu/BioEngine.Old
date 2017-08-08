using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetMenuByKeyQuery : SingleModelQueryBase<Menu>
    {
        public GetMenuByKeyQuery(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}