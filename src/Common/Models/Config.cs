using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_config")]
    public class Config : BaseModel
    {
        [Key]
        [Column("site_id")]
        public int SiteId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("charset")]
        public string Charset { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("head_copy")]
        public string HeadCopy { get; set; }

        [Column("keywords")]
        public string Keywords { get; set; }

        [Column("desc")]
        public string Desc { get; set; }

        [Column("skin")]
        public string Skin { get; set; }

        [Column("default_module")]
        public string DefaultModule { get; set; }

        [Column("admin_module")]
        public string AdminModule { get; set; }

        [Column("forum")]
        public string Forum { get; set; }

        [Column("forum_path")]
        public string ForumPath { get; set; }

        [Required]
        [Column("news_forum")]
        public int NewsForum { get; set; }

        [Required]
        [Column("debug")]
        public int Debug { get; set; }
    }
}