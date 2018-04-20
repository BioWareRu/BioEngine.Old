using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Social.Commands;
using BioEngine.Social.Facebook;

namespace BioEngine.Data.Social.Handlers
{
    internal class PublishFacebookLinkHandler : QueryHandlerBase<PublishFacebookLinkCommand, string>
    {
        private readonly FacebookService _facebookService;

        public PublishFacebookLinkHandler(HandlerContext<PublishFacebookLinkHandler> context,
            FacebookService facebookService) : base(context)
        {
            _facebookService = facebookService;
        }

        protected override Task<string> RunQueryAsync(PublishFacebookLinkCommand message)
        {
            var postId = _facebookService.PostLink(message.Link);

            return postId;
        }
    }
}