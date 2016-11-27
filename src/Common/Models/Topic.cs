using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Topic : ParentModel
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }

        public string Desc { get; set; }

        public override string NewsUrl => "#";
        public override string Icon => Logo;
        public override string DisplayTitle => Title;

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().ToTable("be_nuke_topics");
            modelBuilder.Entity<Topic>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Topic>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<Topic>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Topic>().Property(x => x.Logo).HasColumnName("logo");
            modelBuilder.Entity<Topic>().Property(x => x.Desc).HasColumnName("desc");
        }
    }
}