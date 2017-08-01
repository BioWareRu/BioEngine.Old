using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_articles_cats")]
    public class ArticleCat : ChildModel<int>, ICat<ArticleCat>
    {
        public int Pid { get; set; }

        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]

        public string Url { get; set; }

        [JsonProperty]
        public string Descr { get; set; }


        public string GameOld { get; set; }

        [JsonProperty]
        public string Content { get; set; }


        public int Articles { get; set; }

        [ForeignKey(nameof(Pid))]
        public ArticleCat ParentCat { get; set; }
        
        [InverseProperty(nameof(ParentCat))]
        public List<ArticleCat> Children { get; set; }
    }
}