using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_admins_rights")]
    public class AdminRights : BaseModel<int>
    {
        [Column("uid")]
        [Required]
        public int UId { get; set; }

        [Required]
        public int Admin { get; set; }

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
        public int Admins { get; set; }

        [Required]
        public int Sup { get; set; }
    }
}