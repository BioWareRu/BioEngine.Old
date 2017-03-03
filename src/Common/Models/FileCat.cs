using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Search;

namespace BioEngine.Common.Models
{
    [Table("be_files_cats")]
    public class FileCat : BaseModel<int>, ICat<FileCat>, ISearchModel
    {
        
        public int Pid { get; set; }

        
        public string GameOld { get; set; }

        
        public string Title { get; set; }

        
        public string Descr { get; set; }

        
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public FileCat ParentCat { get; set; }

        [Key]
        
        public override int Id { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<FileCat> Children { get; set; }


        
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
    }
}