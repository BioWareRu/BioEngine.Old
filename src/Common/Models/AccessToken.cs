using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Common.Models
{
    [Table("be_oauth2server_access_tokens")]
    public class AccessToken : BaseModel
    {
        [Key]
        [Column("access_token")]
        public string Token { get; set; }

        public string ClientId { get; set; }

        public int MemberId { get; set; }

        public DateTime Expires { get; set; }

        public string Scope { get; set; }

        [ForeignKey(nameof(MemberId))]
        public User User { get; set; }
    }
}