using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_blocks")]
    public class Block : BaseModel<string>
    {
        [Key]
        [Column("index")]
        public override string Id { get; set; }

        [Required]
        
        public string Content { get; set; }

        [Required]
        
        public int Active { get; set; }
    }
}