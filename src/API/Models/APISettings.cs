using BioEngine.Common.Base;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BioEngine.API.Models
{
    public class APISettings : BaseModel<int>
    {
        [JsonProperty]
        public string FileBrowserUrl { get; }

        public APISettings(IConfiguration configuration, string token)
        {
            FileBrowserUrl = $"{configuration["API_FILE_BROWSER_URL"]}?token={token}";
        }
    }
}