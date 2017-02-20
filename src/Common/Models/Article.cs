using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_articles")]
    public class Article : IChildModel
    {
        [Key]

        public int Id { get; set; }


        public string Url { get; set; }


        public string Source { get; set; }


        public int CatId { get; set; }


        public string Title { get; set; }


        public string Announce { get; set; }


        public string Text { get; set; }


        public int AuthorId { get; set; }


        public int Count { get; set; }


        public int Date { get; set; }


        public int Pub { get; set; }


        public int Fs { get; set; }


        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CatId))]
        public ArticleCat Cat { get; set; }


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