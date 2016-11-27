using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_site_team")]
    public class SiteTeam : BaseModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("member_id")]
        [Required]
        public int MemberId { get; set; }

        [Column("developers")]
        [Required]
        public int Developers { get; set; }

        [Column("games")]
        [Required]
        public int Games { get; set; }

        [Column("news")]
        [Required]
        public int News { get; set; }

        [Column("articles")]
        [Required]
        public int Articles { get; set; }

        [Column("files")]
        [Required]
        public int Files { get; set; }

        [Column("gallery")]
        [Required]
        public int Gallery { get; set; }

        [Column("polls")]
        [Required]
        public int Polls { get; set; }

        [Column("tags")]
        [Required]
        public int Tags { get; set; }

        [Column("active")]
        [Required]
        public int Active { get; set; }
    }
}