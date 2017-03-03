using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_menu")]
    public class Menu : BaseModel<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Key { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        [Required]
        public int Date { get; set; }

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