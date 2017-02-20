using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_articles_cats")]
    public class ArticleCat : ICat<ArticleCat>
    {
        
        public int Pid { get; set; }

        
        public string Title { get; set; }

        
        public string Url { get; set; }

        
        public string Descr { get; set; }

        
        public string GameOld { get; set; }

        
        public string Content { get; set; }

        
        public int Articles { get; set; }

        [ForeignKey(nameof(Pid))]
        public ArticleCat ParentCat { get; set; }

        [Key]
        
        public int Id { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<ArticleCat> Children { get; set; }

        
        public int? GameId { get; set; }

        
        public int? DeveloperId { get; set; }

        
        public int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }

        [NotMapped]
        public ParentModel Parent
        {
            get { return ParentModel.GetParent(this); }
            set { ParentModel.SetParent(this, value); }
        }
    }
}