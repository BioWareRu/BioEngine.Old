using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_settings")]
    public class Settings : BaseModel<int>
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public string Value { get; set; }
    }
}