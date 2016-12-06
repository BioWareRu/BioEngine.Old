using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Article : ChildModel
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }
        public string Source { get; set; }
        public int CatId { get; set; }
        public string Title { get; set; }
        public string Announce { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public int Date { get; set; }
        public int Pub { get; set; }
        public int Fs { get; set; }


        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CatId))]
        public ArticleCat Cat { get; set; }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().ToTable("be_articles");
            modelBuilder.Entity<Article>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Article>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<Article>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<Article>().Property(x => x.TopicId).HasColumnName("topic_id");
            modelBuilder.Entity<Article>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<Article>().Property(x => x.Source).HasColumnName("source");
            modelBuilder.Entity<Article>().Property(x => x.CatId).HasColumnName("cat_id");
            modelBuilder.Entity<Article>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Article>().Property(x => x.Announce).HasColumnName("announce");
            modelBuilder.Entity<Article>().Property(x => x.Text).HasColumnName("text");
            modelBuilder.Entity<Article>().Property(x => x.AuthorId).HasColumnName("author_id");
            modelBuilder.Entity<Article>().Property(x => x.Count).HasColumnName("count");
            modelBuilder.Entity<Article>().Property(x => x.Pub).HasColumnName("pub");
            modelBuilder.Entity<Article>().Property(x => x.Fs).HasColumnName("fs");
            modelBuilder.Entity<Article>().Property(x => x.Date).HasColumnName("date");
        }
    }
}