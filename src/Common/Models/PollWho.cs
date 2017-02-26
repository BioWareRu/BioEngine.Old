using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BioEngine.Common.Models
{
    [Table("be_poll_who")]
    public class PollWho
    {
        [Key]
        public int PollWhoId { get; set; }

        public int PollId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public long VoteDate { get; set; }

        [Column("voteoption")]
        public int VoteOption { get; set; }

        public string Ip { get; set; }
        public string SessionId { get; set; }
    }
}