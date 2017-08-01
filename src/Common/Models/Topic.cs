using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_nuke_topics")]
    public class Topic : ParentModel<int>
    {
        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Logo { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public override ParentType Type { get; } = ParentType.Topic;

        public override string Icon => Logo;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Title;
    }
}