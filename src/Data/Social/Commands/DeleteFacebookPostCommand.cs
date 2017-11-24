using BioEngine.Data.Core;

namespace BioEngine.Data.Social.Commands
{
    public class DeleteFacebookPostCommand : QueryBase<bool>
    {
        public string PostId { get; }

        public DeleteFacebookPostCommand(string postId)
        {
            PostId = postId;
        }
    }
}