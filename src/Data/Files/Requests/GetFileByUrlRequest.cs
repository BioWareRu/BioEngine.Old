using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetFileByUrlRequest : RequestBase<File>
    {
        public GetFileByUrlRequest(IParentModel parent, string catUrl, string url)
        {
            Parent = parent;
            CatUrl = catUrl;
            Url = url;
        }

        public IParentModel Parent { get; }
        public string CatUrl { get; }
        public string Url { get; }
    }
}