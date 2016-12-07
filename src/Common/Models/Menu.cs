using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BioEngine.Common.Base;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    public class Menu : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        [Required]
        public int Date { get; set; }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().ToTable("be_menu");
            modelBuilder.Entity<Menu>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Menu>().Property(x => x.Key).HasColumnName("key");
            modelBuilder.Entity<Menu>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Menu>().Property(x => x.Code).HasColumnName("code");
            modelBuilder.Entity<Menu>().Property(x => x.Date).HasColumnName("date");
        }

        public List<MenuItem> GetMenu()
        {
            return JsonConvert.DeserializeObject<List<MenuItem>>(Code);
        }
    }

    [UsedImplicitly]
    public class MenuItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("items")]
        public List<MenuItem> Children { get; set; }
    }

    public struct MenuLevel
    {
        public int Level;
        public List<MenuItem> Items;
    }
}