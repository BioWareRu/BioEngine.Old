using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_news")]
    public class News : ChildModel<int>
    {
        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Source { get; set; }

        public string GameOld { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string ShortText { get; set; }

        [JsonProperty]
        public string AddText { get; set; }

        [Required]
        [JsonProperty]
        public int AuthorId { get; set; }

        [Column("tid")]
        public int? ForumTopicId { get; set; }

        [Column("pid")]
        public int? ForumPostId { get; set; }

        [Required]
        [JsonProperty]
        public int Sticky { get; set; }

        [Required]
        [JsonProperty]
        public long Date { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        [JsonProperty]
        public long LastChangeDate { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        [Required]
        [JsonProperty]
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
        
        [Required]
        public string FacebookId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public virtual User Author { get; set; }

        public bool HasMore => !string.IsNullOrEmpty(AddText);

        [JsonProperty]
        public override int? GameId { get; set; }

        [JsonProperty]
        public override int? DeveloperId { get; set; }

        [JsonProperty]
        public override int? TopicId { get; set; }

        [JsonProperty]
        public string AuthorName => Author?.Name;

        [JsonProperty]
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
                return Topic?.Title;
            }
        }
    }
}