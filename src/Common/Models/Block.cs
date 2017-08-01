using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_blocks")]
    public class Block : BaseModel<string>
    {
        [Key]
        [Column("index")]
        [JsonProperty]
        public override string Id { get; set; }

        [Required]
        [JsonProperty]
        public string Content { get; set; }

        [Required]
        [JsonProperty]
        public int Active { get; set; }
    }
}