using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class FileCat : ChildModel, ICat<FileCat>
    {
        [Key]
        public int Id { get; set; }

        public int Pid { get; set; }
        public string GameOld { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public FileCat ParentCat { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<FileCat> Children { get; set; }

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