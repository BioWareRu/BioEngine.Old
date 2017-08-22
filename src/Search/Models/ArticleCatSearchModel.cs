using BioEngine.Common.Models;
using Newtonsoft.Json;

namespace BioEngine.Search.Models
{
    public class ArticleCatSearchModel : ArticleCat
    {
        [JsonProperty]
        public override Game Game { get; set; }

        [JsonProperty]
        public override Developer Developer { get; set; }

        [JsonProperty]
        public override Topic Topic { get; set; }

        [JsonProperty]
        public override ArticleCat ParentCat { get; set; }
    }
}