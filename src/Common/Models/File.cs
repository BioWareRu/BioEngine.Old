using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_files")]
    public class File : ChildModel<int>
    {
        [JsonProperty]
        public string Url { get; set; }

        [JsonProperty]
        public int CatId { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public string Announce { get; set; }

        [JsonProperty]
        public string Link { get; set; }

        [JsonProperty]
        public int Size { get; set; }

        [JsonProperty]
        public string YtId { get; set; }


        public int AuthorId { get; set; }


        public int Count { get; set; }

        [JsonProperty]
        public int Date { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }


        [ForeignKey(nameof(CatId))]
        public FileCat Cat { get; set; }

        [NotMapped]
        public double SizeInMb => Math.Round((double) Size / 1024 / 1024, 2);
        
        [NotMapped]
        public override int? TopicId { get; set; }

        [NotMapped]
        public override Topic Topic { get; set; }
    }
}