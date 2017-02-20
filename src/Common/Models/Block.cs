using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_blocks")]
    public class Block : BaseModel
    {
        [Key]
        
        public string Index { get; set; }

        [Required]
        
        public string Content { get; set; }

        [Required]
        
        public int Active { get; set; }
    }
}