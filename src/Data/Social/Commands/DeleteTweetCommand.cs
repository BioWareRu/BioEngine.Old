using BioEngine.Data.Core;

namespace BioEngine.Data.Social.Commands
{
    public class DeleteTweetCommand : QueryBase<bool>
    {
        public long TweetId { get; }

        public DeleteTweetCommand(long tweetId)
        {
            TweetId = tweetId;
        }
    }
}