using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Social.Commands;
using BioEngine.Social.Facebook;
using BioEngine.Social.Twitter;
using JetBrains.Annotations;

namespace BioEngine.Data.Social.Handlers
{
    [UsedImplicitly]
    internal class DeleteFacebookPostHandler : QueryHandlerBase<DeleteFacebookPostCommand, bool>
    {
        private readonly FacebookService _facebookService;

        public DeleteFacebookPostHandler(HandlerContext<DeleteFacebookPostHandler> context,
            FacebookService facebookService) : base(context)
        {
            _facebookService = facebookService;
        }

        protected override async Task<bool> RunQueryAsync(DeleteFacebookPostCommand command)
        {
            var result = await _facebookService.DeletePost(command.PostId);

            return result;
        }
    }
}