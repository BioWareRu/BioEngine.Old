using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetGameByUrlQuery : SingleModelQueryBase<Game>
    {
        public GetGameByUrlQuery(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}