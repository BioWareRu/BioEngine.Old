using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class GalleryCat : ICat<GalleryCat>
    {
        public const int PicsOnPage = 24;

        public int Pid { get; set; }
        public string GameOld { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public GalleryCat ParentCat { get; set; }

        [Key]
        public int Id { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<GalleryCat> Children { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }

        [NotMapped]
        public int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [NotMapped]
        public Topic Topic { get; set; }

        [NotMapped]
        public ParentModel Parent
        {
            get { return ParentModel.GetParent(this); }
            set { ParentModel.SetParent(this, value); }
        }

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GalleryCat>().ToTable("be_gallery_cats");
            modelBuilder.Entity<GalleryCat>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<GalleryCat>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<GalleryCat>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<GalleryCat>().Property(x => x.GameOld).HasColumnName("game_old");
            modelBuilder.Entity<GalleryCat>().Property(x => x.Pid).HasColumnName("pid");
            modelBuilder.Entity<GalleryCat>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<GalleryCat>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<GalleryCat>().Property(x => x.Url).HasColumnName("url");
        }
    }
}