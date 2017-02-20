using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_games")]
    public class Game : ParentModel
    {
        [Key]
       public override int Id { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string AdminTitle { get; set; }

        public string Genre { get; set; }

        public string ReleaseDate { get; set; }

        public string Platforms { get; set; }

        public string Dev { get; set; }

        public string Desc { get; set; }

        public string Keywords { get; set; }

        public string Publisher { get; set; }

        public string Localizator { get; set; }

        [Required]
        public int Status { get; set; }

        public string Logo { get; set; }

        public string SmallLogo { get; set; }

        [Required]
        public int Date { get; set; }

        [Column("tweettag")]
        public string TweetTag { get; set; }


        public string Info { get; set; }

        public string Specs { get; set; }

        [Required]
        public int RatePos { get; set; }

        [Required]
        public int RateNeg { get; set; }

        public string VotedUsers { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        public override ParentType Type { get; } = ParentType.Game;
        public override string ParentUrl => Url;
        public override string DisplayTitle => Title;
    }
}