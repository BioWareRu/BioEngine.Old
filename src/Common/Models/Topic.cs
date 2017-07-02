using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_nuke_topics")]
    public class Topic : ParentModel<int>
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }

        public string Desc { get; set; }

        public override ParentType Type { get; } = ParentType.Topic;

        public override string Icon => Logo;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Title;
    }
}