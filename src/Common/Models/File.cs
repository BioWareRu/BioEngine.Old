using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_files")]
    public class File : IChildModel
    {
        [Key]
        
        public int Id { get; set; }

        
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

        [NotMapped]
        public ParentModel Parent
        {
            get { return ParentModel.GetParent(this); }
            set { ParentModel.SetParent(this, value); }
        }
    }
}