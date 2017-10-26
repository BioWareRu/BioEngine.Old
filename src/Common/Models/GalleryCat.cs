using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_gallery_cats")]
    public class GalleryCat : ChildModel<int>, ICat<GalleryCat, GalleryPic>
    {
        public const int PicsOnPage = 24;

        [JsonProperty]
        [Column("pid")]
        public int? CatId { get; set; }

        public string GameOld { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public string Url { get; set; }

        [ForeignKey(nameof(CatId))]
        public virtual GalleryCat ParentCat { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<GalleryCat> Children { get; set; }

        public IEnumerable<GalleryPic> Items { get; set; } = new List<GalleryPic>();

        [NotMapped]
        public override int? TopicId { get; set; }

        [NotMapped]
        public override Topic Topic { get; set; }
    }
}