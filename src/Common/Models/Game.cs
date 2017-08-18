using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_games")]
    public class Game : ParentModel<int>
    {
        [Required]
        [JsonProperty]
        public int DeveloperId { get; set; }

        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string AdminTitle { get; set; }

        [JsonProperty]
        public string Genre { get; set; }

        [JsonProperty]
        public string ReleaseDate { get; set; }

        [JsonProperty]
        public string Platforms { get; set; }

        public string Dev { get; set; }

        [JsonProperty]
        public string Desc { get; set; }
        public string NewsDesc { get; set; }

        public string Keywords { get; set; }

        [JsonProperty]
        public string Publisher { get; set; }

        [JsonProperty]
        public string Localizator { get; set; }

        [Required]
        public int Status { get; set; }

        [JsonProperty]
        public string Logo { get; set; }

        [JsonProperty]
        public string SmallLogo { get; set; }

        [Required]
        public int Date { get; set; }

        [Column("tweettag")]
        [JsonProperty]
        public string TweetTag { get; set; }

        [JsonProperty]
        public string Info { get; set; }

        [JsonProperty]
        public string Specs { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [JsonProperty]
        public override ParentType Type { get; } = ParentType.Game;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Title;
        public override string Icon => SmallLogo;
        public override string TwitterTag => TweetTag;
    }
}