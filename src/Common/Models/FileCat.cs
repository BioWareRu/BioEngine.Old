using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_files_cats")]
    public class FileCat : ChildModel<int>, ICat<FileCat>
    {
        public int Pid { get; set; }

        
        public string GameOld { get; set; }

        
        public string Title { get; set; }

        
        public string Descr { get; set; }

        
        public string Url { get; set; }

        [ForeignKey(nameof(Pid))]
        public FileCat ParentCat { get; set; }

        [InverseProperty(nameof(ParentCat))]
        public List<FileCat> Children { get; set; }

        [NotMapped]
        public override int? TopicId { get; set; }

        [NotMapped]
        public override Topic Topic { get; set; }
    }
}