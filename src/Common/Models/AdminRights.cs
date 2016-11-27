using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_admins_rights")]
    public class AdminRights : BaseModel
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("uid")]
        [Required]
        public int UId { get; set; }

        [Column("admin")]
        [Required]
        public int Admin { get; set; }

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

        [Column("admins")]
        [Required]
        public int Admins { get; set; }

        [Column("sup")]
        [Required]
        public int Sup { get; set; }
    }
}