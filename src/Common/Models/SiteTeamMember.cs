using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_site_team")]
    public class SiteTeamMember : BaseModel<int>
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int Developers { get; set; }

        [Required]
        public int Games { get; set; }

        [Required]
        public int News { get; set; }

        [Required]
        public int Articles { get; set; }

        [Required]
        public int Files { get; set; }

        [Required]
        public int Gallery { get; set; }

        [Required]
        public int Polls { get; set; }

        [Required]
        public int Tags { get; set; }

        [Required]
        public int Active { get; set; }

        [ForeignKey(nameof(MemberId))]
        public User User { get; set; }
    }
}