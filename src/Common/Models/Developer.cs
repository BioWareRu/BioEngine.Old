using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_developers")]
    public class Developer : ParentModel<int>
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public string Desc { get; set; }

        public string Logo { get; set; }

        [Required]
        public int FoundYear { get; set; }

        public string Location { get; set; }

        public string Peoples { get; set; }

        public string Site { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        public override ParentType Type { get; } = ParentType.Developer;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Name;
        public override string Icon => Logo;
    }
}