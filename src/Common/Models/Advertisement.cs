using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    public class Advertisement : BaseModel
    {
        public long AdId { get; set; }
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

        public AdvetisementImages Images
        {
            get
            {
                return JsonConvert.DeserializeObject<AdvetisementImages>(AdImages);
            }
        }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>().ToTable("be_core_advertisements");
            modelBuilder.Entity<Advertisement>().HasKey(nameof(AdId));
            modelBuilder.Entity<Advertisement>().Property(x => x.AdId).HasColumnName("ad_id");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdLocation).HasColumnName("ad_location");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdHtml).HasColumnName("ad_html");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdImages).HasColumnName("ad_images");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdLink).HasColumnName("ad_link");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdImpressions).HasColumnName("ad_impressions");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdClicks).HasColumnName("ad_clicks");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdExempt).HasColumnName("ad_exempt");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdActive).HasColumnName("ad_active");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdHtmlHttps).HasColumnName("ad_html_https");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdStart).HasColumnName("ad_start");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdEnd).HasColumnName("ad_end");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdMaximumValue).HasColumnName("ad_maximum_value");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdMaximumUnit).HasColumnName("ad_maximum_unit");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdAdditionalSettings).HasColumnName("ad_additional_settings");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdHtmlHttpsSet).HasColumnName("ad_html_https_set");
            modelBuilder.Entity<Advertisement>().Property(x => x.AdMember).HasColumnName("ad_member");
        }
    }

    public struct AdvetisementImages
    {
        public string Large
        {
            get; set;
        }

    }
}
