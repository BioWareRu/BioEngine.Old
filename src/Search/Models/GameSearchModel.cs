using BioEngine.Common.Models;
using Newtonsoft.Json;

namespace BioEngine.Search.Models
{
    public class GameSearchModel : Game
    {
        [JsonProperty]
        public override Developer Developer { get; set; }
    }
}