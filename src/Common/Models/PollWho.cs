using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class PollWho
    {
        public int PollWhoId { get; set; }
        public int PollId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public int VoteDate { get; set; }
        public int VoteOption { get; set; }
        public string Ip { get; set; }
        public string SessionId { get; set; }

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PollWho>().ToTable("be_poll_who");
            modelBuilder.Entity<PollWho>().HasKey(nameof(PollWhoId));
            modelBuilder.Entity<PollWho>().Property(x => x.PollWhoId).HasColumnName("poll_who_id");
            modelBuilder.Entity<PollWho>().Property(x => x.PollId).HasColumnName("poll_id");
            modelBuilder.Entity<PollWho>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<PollWho>().Property(x => x.Login).HasColumnName("login");
            modelBuilder.Entity<PollWho>().Property(x => x.VoteDate).HasColumnName("vote_date");
            modelBuilder.Entity<PollWho>().Property(x => x.VoteOption).HasColumnName("voteoption");
            modelBuilder.Entity<PollWho>().Property(x => x.Ip).HasColumnName("ip");
            modelBuilder.Entity<PollWho>().Property(x => x.SessionId).HasColumnName("session_id");
        }
    }
}