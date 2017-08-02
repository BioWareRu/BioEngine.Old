using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Requests
{
    public class GetParentByUrlRequest : RequestBase<IParentModel>
    {
        public string Url { get; }

        public GetParentByUrlRequest(string url)
        {
            Url = url;
        }
    }
}