using BioEngine.Common.Base;

namespace BioEngine.Social.Twitter
{
    public class TwitterException : UserException
    {
        public TwitterException(string message) : base(message)
        {
        }
    }
}