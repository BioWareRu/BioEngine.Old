using BioEngine.Common.Base;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BioEngine.API.Models
{
    public class APISettings : BaseModel<int>
    {
        [JsonProperty("fileBrowserUrl")]
        public string FileBrowserUrlWithToken => $"{FileBrowserUrl}?token={UserToken}";

        public string FileBrowserUrl { get; set; }
        public string UserToken { get; set; }
    }
}