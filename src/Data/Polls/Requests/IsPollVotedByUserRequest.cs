using BioEngine.Data.Core;

namespace BioEngine.Data.Polls.Requests
{
    public class IsPollVotedByUserRequest : RequestBase<bool>
    {
        public IsPollVotedByUserRequest(int pollId, int userId, string ipAddress, string sessionId)
        {
            PollId = pollId;
            UserId = userId;
            IpAddress = ipAddress;
            SessionId = sessionId;
        }

        public int PollId { get; }
        public int UserId { get; }
        public string IpAddress { get; }
        public string SessionId { get; }
    }
}