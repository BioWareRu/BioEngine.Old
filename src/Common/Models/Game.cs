using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Game : ParentModel
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string AdminTitle { get; set; }

        public string Genre { get; set; }

        public string ReleaseDate { get; set; }

        public string Platforms { get; set; }

        public string Dev { get; set; }

        public string Desc { get; set; }

        public string Keywords { get; set; }

        public string Publisher { get; set; }

        public string Localizator { get; set; }

        [Required]
        public int Status { get; set; }

        public string Logo { get; set; }

        public string SmallLogo { get; set; }

        [Required]
        public int Date { get; set; }

        public string TweetTag { get; set; }


        public string Info { get; set; }

        public string Specs { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        public override ParentType Type { get; } = ParentType.Game;
        public override string NewsUrl => "#";
        public override string Icon => SmallLogo;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Title;

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("be_games");
            modelBuilder.Entity<Game>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Game>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<Game>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<Game>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Game>().Property(x => x.AdminTitle).HasColumnName("admin_title");
            modelBuilder.Entity<Game>().Property(x => x.Genre).HasColumnName("genre");
            modelBuilder.Entity<Game>().Property(x => x.ReleaseDate).HasColumnName("release_date");
            modelBuilder.Entity<Game>().Property(x => x.Platforms).HasColumnName("platforms");
            modelBuilder.Entity<Game>().Property(x => x.Dev).HasColumnName("dev");
            modelBuilder.Entity<Game>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<Game>().Property(x => x.Keywords).HasColumnName("keywords");
            modelBuilder.Entity<Game>().Property(x => x.Publisher).HasColumnName("publisher");
            modelBuilder.Entity<Game>().Property(x => x.Localizator).HasColumnName("localizator");
            modelBuilder.Entity<Game>().Property(x => x.Status).HasColumnName("status");
            modelBuilder.Entity<Game>().Property(x => x.Logo).HasColumnName("logo");
            modelBuilder.Entity<Game>().Property(x => x.SmallLogo).HasColumnName("small_logo");
            modelBuilder.Entity<Game>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<Game>().Property(x => x.TweetTag).HasColumnName("tweettag");
            modelBuilder.Entity<Game>().Property(x => x.Info).HasColumnName("info");
            modelBuilder.Entity<Game>().Property(x => x.Specs).HasColumnName("specs");
            modelBuilder.Entity<Game>().Property(x => x.RatePos).HasColumnName("rate_pos");
            modelBuilder.Entity<Game>().Property(x => x.RateNeg).HasColumnName("rate_neg");
            modelBuilder.Entity<Game>().Property(x => x.VotedUsers).HasColumnName("voted_users");
        }
    }
}