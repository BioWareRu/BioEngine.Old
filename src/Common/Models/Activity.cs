using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_activity")]
    public class Activity : BaseModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("page")]
        [Url]
        public string Page { get; set; }

        [Column("time")]
        [Required]
        public int Time { get; set; }
    }
}