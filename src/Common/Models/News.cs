using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class News : ChildModel
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string Source { get; set; }

        public string GameOld { get; set; }

        public string Title { get; set; }

        public string ShortText { get; set; }

        public string AddText { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public int ForumTopicId { get; set; }

        [Required]
        public int ForumPostId { get; set; }

        [Required]
        public int Sticky { get; set; }

        [Required]
        public int Date { get; set; }

        public int LastChangeDate { get; set; }

        [Required]
        public int Pub { get; set; }

        public string AddGames { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [Required]
        public int Comments { get; set; }

        [Required]
        public long TwitterId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        public bool HasMore => !string.IsNullOrEmpty(AddText);

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().ToTable("be_news");
            modelBuilder.Entity<News>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<News>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<News>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<News>().Property(x => x.TopicId).HasColumnName("topic_id");
            modelBuilder.Entity<News>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<News>().Property(x => x.Source).HasColumnName("source");
            modelBuilder.Entity<News>().Property(x => x.GameOld).HasColumnName("game_old");
            modelBuilder.Entity<News>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<News>().Property(x => x.ShortText).HasColumnName("short_text");
            modelBuilder.Entity<News>().Property(x => x.AddText).HasColumnName("add_text");
            modelBuilder.Entity<News>().Property(x => x.AuthorId).HasColumnName("author_id");
            modelBuilder.Entity<News>().Property(x => x.ForumTopicId).HasColumnName("tid");
            modelBuilder.Entity<News>().Property(x => x.ForumPostId).HasColumnName("pid");
            modelBuilder.Entity<News>().Property(x => x.Sticky).HasColumnName("sticky");
            modelBuilder.Entity<News>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<News>().Property(x => x.LastChangeDate).HasColumnName("last_change_date");
            modelBuilder.Entity<News>().Property(x => x.Pub).HasColumnName("pub");
            modelBuilder.Entity<News>().Property(x => x.AddGames).HasColumnName("addgames");
            modelBuilder.Entity<News>().Property(x => x.RatePos).HasColumnName("rate_pos");
            modelBuilder.Entity<News>().Property(x => x.RateNeg).HasColumnName("rate_neg");
            modelBuilder.Entity<News>().Property(x => x.VotedUsers).HasColumnName("voted_users");
            modelBuilder.Entity<News>().Property(x => x.Comments).HasColumnName("comments");
            modelBuilder.Entity<News>().Property(x => x.TwitterId).HasColumnName("twitter_id");
        }
    }
}