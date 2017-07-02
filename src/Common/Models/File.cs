using System;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_files")]
    public class File : ChildModel<int>
    {
        public string Url { get; set; }

        public int CatId { get; set; }


        public string Title { get; set; }


        public string Desc { get; set; }


        public string Announce { get; set; }

        public string Link { get; set; }


        public int Size { get; set; }

        public string YtId { get; set; }


        public int AuthorId { get; set; }


        public int Count { get; set; }

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