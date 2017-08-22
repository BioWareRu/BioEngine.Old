using BioEngine.Common.Models;
using Newtonsoft.Json;

namespace BioEngine.Search.Models
{
    public class ArticleSearchModel : Article
    {
        [JsonProperty]
        public override Game Game { get; set; }

        [JsonProperty]
        public override Developer Developer { get; set; }

        [JsonProperty]
        public override Topic Topic { get; set; }

        [JsonProperty]
        public override ArticleCat Cat { get; set; }
    }
}