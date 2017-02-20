using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_settings")]
    public class Settings : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Desc { get; set; }

        public string Value { get; set; }
    }
}