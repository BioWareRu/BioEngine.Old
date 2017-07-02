using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_core_advertisements")]
    public class Advertisement : BaseModel<long>
    {
        [Key]
        [Column("ad_id")]
        public override long Id { get; set; }

        public string AdLocation { get; set; }

        public string AdHtml { get; set; }

        public string AdImages { get; set; }

        public string AdLink { get; set; }


        public ulong AdImpressions { get; set; }


        public int AdClicks { get; set; }


        public string AdExempt { get; set; }


        public int AdActive { get; set; }


        public string AdHtmlHttps { get; set; }


        public int AdStart { get; set; }


        public int AdEnd { get; set; }


        public int AdMaximumValue { get; set; }


        public string AdMaximumUnit { get; set; }


        public string AdAdditionalSettings { get; set; }


        public bool AdHtmlHttpsSet { get; set; }


        public uint? AdMember { get; set; }

        public AdvetisementImages Images => JsonConvert.DeserializeObject<AdvetisementImages>(AdImages);
    }

    public struct AdvetisementImages
    {
        public string Large { get; set; }
    }
}