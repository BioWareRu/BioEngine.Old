using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class File : ChildModel
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }
        public string GameOld { get; set; }
        public int CatId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Announce { get; set; }
        public string FileName { get; set; }
        public string Link { get; set; }
        public int Size { get; set; }
        public int Stream { get; set; }
        public string StreamFile { get; set; }
        public int YtStatus { get; set; }
        public string YtTitle { get; set; }
        public string YtUrl { get; set; }
        public string YtId { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public int Date { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }


        [ForeignKey(nameof(CatId))]
        public FileCat Cat { get; set; }

        [NotMapped]
        public double SizeInMb => Math.Round((double) Size/1024/1024, 2);

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>().ToTable("be_files");
            modelBuilder.Entity<File>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<File>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<File>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<File>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<File>().Property(x => x.GameOld).HasColumnName("game_old");
            modelBuilder.Entity<File>().Property(x => x.CatId).HasColumnName("cat_id");
            modelBuilder.Entity<File>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<File>().Property(x => x.Announce).HasColumnName("announce");
            modelBuilder.Entity<File>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<File>().Property(x => x.AuthorId).HasColumnName("author_id");
            modelBuilder.Entity<File>().Property(x => x.Count).HasColumnName("count");
            modelBuilder.Entity<File>().Property(x => x.FileName).HasColumnName("file");
            modelBuilder.Entity<File>().Property(x => x.Size).HasColumnName("size");
            modelBuilder.Entity<File>().Property(x => x.Stream).HasColumnName("stream");
            modelBuilder.Entity<File>().Property(x => x.StreamFile).HasColumnName("streamfile");
            modelBuilder.Entity<File>().Property(x => x.YtStatus).HasColumnName("yt_status");
            modelBuilder.Entity<File>().Property(x => x.YtTitle).HasColumnName("yt_title");
            modelBuilder.Entity<File>().Property(x => x.YtUrl).HasColumnName("yt_url");
            modelBuilder.Entity<File>().Property(x => x.YtId).HasColumnName("yt_id");
        }
    }
}