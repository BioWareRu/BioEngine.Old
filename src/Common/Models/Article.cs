using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_articles")]
    public class Article : ChildModel<int>
    {
        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public string Source { get; set; }

        [JsonProperty]
        public int? CatId { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Announce { get; set; }

        [JsonProperty]
        public string Text { get; set; }

        [JsonProperty]
        public int AuthorId { get; set; }


        public int Count { get; set; }

        [JsonProperty]
        public int Date { get; set; }

        [JsonProperty]
        public int Pub { get; set; }


        public int Fs { get; set; }


        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CatId))]
        public virtual ArticleCat Cat { get; set; }
        
        [JsonProperty]
        public string AuthorName => Author?.Name;
        
        [JsonProperty]
        public string CatName => Cat?.Title;

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