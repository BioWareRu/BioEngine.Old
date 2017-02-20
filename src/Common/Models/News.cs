using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_news")]
    public class News : IChildModel
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string Source { get; set; }

        public string GameOld { get; set; }

        public string Title { get; set; }

        public string ShortText { get; set; }

        public string AddText { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Column("tid")]
        public int ForumTopicId { get; set; }

        [Required]
        [Column("pid")]
        public int ForumPostId { get; set; }

        [Required]
        public int Sticky { get; set; }

        [Required]
        public int Date { get; set; }

        public int LastChangeDate { get; set; }

        [Required]
        public int Pub { get; set; }

        [Column("addgames")]
        public string AddGames { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [Required]
        public int Comments { get; set; }

        [Required]
        public long TwitterId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public bool HasMore => !string.IsNullOrEmpty(AddText);

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