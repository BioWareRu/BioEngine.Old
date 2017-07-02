using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_articles")]
    public class Article : ChildModel<int>
    {
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
    }
}