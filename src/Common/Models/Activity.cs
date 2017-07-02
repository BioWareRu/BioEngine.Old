using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_activity")]
    public class Activity : BaseModel<int>
    {
        [Required]
        public int UserId { get; set; }

        [Url]
        public string Page { get; set; }

        [Required]
        public int Time { get; set; }
    }
}