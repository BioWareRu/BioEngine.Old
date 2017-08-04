using BioEngine.Data.Core;

namespace BioEngine.Data.Polls.Commands
{
    public class PollVoteCommand : CommandBase
    {
        public PollVoteCommand(int pollId, int voteId, string ipAddress, string sessionId, string login, int userId)
        {
            PollId = pollId;
            VoteId = voteId;
            IpAddress = ipAddress;
            SessionId = sessionId;
            Login = login;
            UserId = userId;
        }

        public int PollId { get; }
        public int VoteId { get; }
        public string IpAddress { get; }
        public string SessionId { get; }
        public string Login { get; }
        public int UserId { get; }
    }
}