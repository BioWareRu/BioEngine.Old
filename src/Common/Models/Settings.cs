using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Settings : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Desc { get; set; }

        public string Value { get; set; }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Settings>().ToTable("be_settings");
            modelBuilder.Entity<Settings>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Settings>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Settings>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Settings>().Property(x => x.Type).HasColumnName("type");
            modelBuilder.Entity<Settings>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<Settings>().Property(x => x.Value).HasColumnName("value");
        }
    }
}