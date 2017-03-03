using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Search;
using JsonApiDotNetCore.Models;

namespace BioEngine.Common.Models
{
    [Table("be_news")]
    public class News : BaseModel<int>, IChildModel, ISearchModel
    {
        [Key]
        public override int Id { get; set; }

        [Attr("url")]
        public string Url { get; set; }

        [Attr("source")]
        public string Source { get; set; }

        public string GameOld { get; set; }

        [Attr("title")]
        public string Title { get; set; }

        [Attr("shortText")]
        public string ShortText { get; set; }

        [Attr("addText")]
        public string AddText { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Column("tid")]
        public int ForumTopicId { get; set; }

        [Required]
        [Column("pid")]
        public int ForumPostId { get; set; }

        [Required]
        [Attr("sticky")]
        public int Sticky { get; set; }

        [Required]
        [Attr("date")]
        public int Date { get; set; }

        [Attr("lastChangeDate")]
        public int LastChangeDate { get; set; }

        [Required]
        [Attr("pub")]
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
        [HasOne("author")]
        public virtual User Author { get; set; }

        public bool HasMore => !string.IsNullOrEmpty(AddText);

        [Attr("gameId")]
        public int? GameId { get; set; }

        [Attr("developerId")]
        public int? DeveloperId { get; set; }

        [Attr("topicId")]
        public int? TopicId { get; set; }

        [Attr("authorName")]
        public string AuthorName => Author?.Name;

        [Attr("parentName")]
        public string ParentName
        {
            get
            {
                if (Game != null)
                {
                    return Game.Title;
                }
                if (Developer != null)
                {
                    return Developer.Name;
                }
                if (Topic != null)
                {
                    return Topic.Title;
                }
                return null;
            }
        }

        [ForeignKey(nameof(GameId))]
        [HasOne("game")]
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        [HasOne("developer")]
        public virtual Developer Developer { get; set; }

        [ForeignKey(nameof(TopicId))]
        [HasOne("topic")]
        public virtual Topic Topic { get; set; }
    }
}