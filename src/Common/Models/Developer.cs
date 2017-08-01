using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_developers")]
    public class Developer : ParentModel<int>
    {
        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Info { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public string Logo { get; set; }

        [Required]
        [JsonProperty]
        public int FoundYear { get; set; }

        [JsonProperty]
        public string Location { get; set; }

        [JsonProperty]
        public string Peoples { get; set; }

        [JsonProperty]
        public string Site { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [JsonProperty]
        public override ParentType Type { get; } = ParentType.Developer;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Name;
        public override string Icon => Logo;
    }
}