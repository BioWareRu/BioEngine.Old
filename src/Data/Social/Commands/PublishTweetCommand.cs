using BioEngine.Data.Core;

namespace BioEngine.Data.Social.Commands
{
    public class PublishTweetCommand : QueryBase<long>
    {
        public string Text { get; }

        public PublishTweetCommand(string text)
        {
            Text = text;
        }
    }
}