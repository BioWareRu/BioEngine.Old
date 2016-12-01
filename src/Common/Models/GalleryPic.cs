using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class GalleryPic : ChildModel
    {
        [Key]
        public int Id { get; set; }
        public int CatId { get; set; }
        public string GameOld { get; set; }
        public string FilesJson { get; set; }
        public string Desc { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public int Date { get; set; }
        public int Pub { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CatId))]
        public GalleryCat Cat { get; set; }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GalleryPic>().ToTable("be_gallery");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.GameOld).HasColumnName("game_old");
            modelBuilder.Entity<GalleryPic>().Property(x => x.FilesJson).HasColumnName("files");
            modelBuilder.Entity<GalleryPic>().Property(x => x.CatId).HasColumnName("cat_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<GalleryPic>().Property(x => x.AuthorId).HasColumnName("author_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Count).HasColumnName("count");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Pub).HasColumnName("pub");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Date).HasColumnName("date");
        }
    }
}