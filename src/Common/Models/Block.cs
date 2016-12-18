using System.ComponentModel.DataAnnotations;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class Block : BaseModel
    {
        [Key]
        public string Index { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Active { get; set; }

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>().ToTable("be_blocks");
            modelBuilder.Entity<Block>().Property(x => x.Index).HasColumnName("index");
            modelBuilder.Entity<Block>().Property(x => x.Content).HasColumnName("content");
            modelBuilder.Entity<Block>().Property(x => x.Active).HasColumnName("active");
        }
    }
}