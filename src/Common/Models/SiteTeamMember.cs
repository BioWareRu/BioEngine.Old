using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_site_team")]
    public class SiteTeamMember : BaseModel<int>
    {
        [Required]
        [JsonProperty]
        public int MemberId { get; set; }

        [Required]
        [JsonProperty]
        public int Developers { get; set; }

        [Required]
        [JsonProperty]
        public int Games { get; set; }

        [Required]
        [JsonProperty]
        public int News { get; set; }

        [Required]
        [JsonProperty]
        public int Articles { get; set; }

        [Required]
        [JsonProperty]
        public int Files { get; set; }

        [Required]
        [JsonProperty]
        public int Gallery { get; set; }

        [Required]
        [JsonProperty]
        public int Polls { get; set; }

        [Required]
        [JsonProperty]
        public int Tags { get; set; }

        [Required]
        [JsonProperty]
        public int Active { get; set; }

        [ForeignKey(nameof(MemberId))]
        public User User { get; set; }
    }
}