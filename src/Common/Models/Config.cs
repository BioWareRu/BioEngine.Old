using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_config")]
    public class Config : BaseModel<int>
    {
        [Key]
        [Column("site_id")]
        public override int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Charset { get; set; }

        public string Author { get; set; }

        public string HeadCopy { get; set; }

        public string Keywords { get; set; }

        public string Desc { get; set; }

        public string Skin { get; set; }

        public string DefaultModule { get; set; }

        public string AdminModule { get; set; }

        public string Forum { get; set; }

        public string ForumPath { get; set; }

        [Required]
        public int NewsForum { get; set; }

        [Required]
        public int Debug { get; set; }
    }
}