using System.Threading.Tasks;
using BioEngine.Common.Ipb;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class DeleteNewsForumTopicHandler : CommandHandlerBase<DeleteNewsForumTopicCommand>
    {
        private readonly IPBApiHelper _ipbApiHelper;

        public DeleteNewsForumTopicHandler(HandlerContext<DeleteNewsForumTopicHandler> context,
            IPBApiHelper ipbApiHelper) : base(context)
        {
            _ipbApiHelper = ipbApiHelper;
        }

        protected override async Task ExecuteCommand(DeleteNewsForumTopicCommand command)
        {
            var result = await _ipbApiHelper.DeleteNewsTopic(command.News);
            if (result)
            {
                command.News.ForumTopicId = 0;
            }
        }
    }
}