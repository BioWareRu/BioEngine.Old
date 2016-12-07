using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class FileCat : IChildModel, ICat<FileCat>
    {
        public int Pid { get; set; }
        public string GameOld { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public FileCat ParentCat { get; set; }

        [Key]
        public int Id { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<FileCat> Children { get; set; }


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

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileCat>().ToTable("be_files_cats");
            modelBuilder.Entity<FileCat>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<FileCat>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<FileCat>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<FileCat>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<FileCat>().Property(x => x.Pid).HasColumnName("pid");
            modelBuilder.Entity<FileCat>().Property(x => x.Descr).HasColumnName("descr");
            modelBuilder.Entity<FileCat>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<FileCat>().Property(x => x.GameOld).HasColumnName("game_old");
        }
    }
}