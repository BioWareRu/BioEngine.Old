using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetParentByUrlQuery : SingleModelQueryBase<IParentModel>
    {
        public string Url { get; }

        public GetParentByUrlQuery(string url)
        {
            Url = url;
        }
    }
}