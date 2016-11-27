using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class User : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int GroupId { get; set; }
        public string EMail { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhotoUrl { get; set; }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("be_core_members");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("member_id");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<User>().Property(x => x.GroupId).HasColumnName("member_group_id");
            modelBuilder.Entity<User>().Property(x => x.EMail).HasColumnName("email");
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasColumnName("members_pass_hash");
            modelBuilder.Entity<User>().Property(x => x.PasswordSalt).HasColumnName("members_pass_salt");
            modelBuilder.Entity<User>().Property(x => x.PhotoUrl).HasColumnName("pp_thumb_photo");
        }
    }
}