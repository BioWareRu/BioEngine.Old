using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Search;

namespace BioEngine.Common.Models
{
    [Table("be_gallery_cats")]
    public class GalleryCat : ICat<GalleryCat>, ISearchModel
    {
        public const int PicsOnPage = 24;

        
        public int Pid { get; set; }

        
        public string GameOld { get; set; }

        
        public string Title { get; set; }

        
        public string Desc { get; set; }

        
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public GalleryCat ParentCat { get; set; }

        [Key]
        
        public int Id { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<GalleryCat> Children { get; set; }

        
        public int? GameId { get; set; }

        
        public int? DeveloperId { get; set; }

        [NotMapped]
        public int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [NotMapped]
        public Topic Topic { get; set; }

        public async Task<ParentModel> Parent(ParentEntityProvider parentEntityProvider)
        {
            return await parentEntityProvider.GetModelParent(this);
        }
    }
}