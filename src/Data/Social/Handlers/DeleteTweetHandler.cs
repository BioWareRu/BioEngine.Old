using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Social.Commands;
using JetBrains.Annotations;
using Social;

namespace BioEngine.Data.Social.Handlers
{
    [UsedImplicitly]
    internal class DeleteTweetHandler : QueryHandlerBase<DeleteTweetCommand, bool>
    {
        private readonly TwitterService _twitterService;

        public DeleteTweetHandler(HandlerContext<DeleteTweetHandler> context,
            TwitterService twitterService) : base(context)
        {
            _twitterService = twitterService;
        }

        protected override async Task<bool> RunQuery(DeleteTweetCommand command)
        {
            var result = await _twitterService.DeleteTweet(command.TweetId);

            return result;
        }
    }
}