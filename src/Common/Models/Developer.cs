using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Developer : ParentModel
    {
        [Key]
        public override int Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public string Desc { get; set; }

        public string Logo { get; set; }

        [Required]
        public int FoundYear { get; set; }

        public string Location { get; set; }

        public string Peoples { get; set; }

        public string Site { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        public override ParentType Type { get; } = ParentType.Developer;
        public override string NewsUrl => "#";
        public override string Icon => Logo;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Name;

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Developer>().ToTable("be_developers");
            modelBuilder.Entity<Developer>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Developer>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<Developer>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Developer>().Property(x => x.Info).HasColumnName("info");
            modelBuilder.Entity<Developer>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<Developer>().Property(x => x.Logo).HasColumnName("logo");
            modelBuilder.Entity<Developer>().Property(x => x.FoundYear).HasColumnName("found_year");
            modelBuilder.Entity<Developer>().Property(x => x.Location).HasColumnName("location");
            modelBuilder.Entity<Developer>().Property(x => x.Peoples).HasColumnName("peoples");
            modelBuilder.Entity<Developer>().Property(x => x.Site).HasColumnName("site");
            modelBuilder.Entity<Developer>().Property(x => x.RatePos).HasColumnName("rate_pos");
            modelBuilder.Entity<Developer>().Property(x => x.RateNeg).HasColumnName("rate_neg");
            modelBuilder.Entity<Developer>().Property(x => x.VotedUsers).HasColumnName("voted_users");
        }
    }
}