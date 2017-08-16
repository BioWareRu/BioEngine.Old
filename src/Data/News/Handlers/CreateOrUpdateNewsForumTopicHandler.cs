using System.Threading.Tasks;
using BioEngine.Common.Ipb;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class CreateOrUpdateNewsForumTopicHandler : CommandHandlerBase<CreateOrUpdateNewsForumTopicCommand>
    {
        private readonly IPBApiHelper _ipbApiHelper;

        public CreateOrUpdateNewsForumTopicHandler(HandlerContext<CreateOrUpdateNewsForumTopicHandler> context,
            IPBApiHelper ipbApiHelper) : base(context)
        {
            _ipbApiHelper = ipbApiHelper;
        }

        protected override async Task ExecuteCommand(CreateOrUpdateNewsForumTopicCommand command)
        {
            var result = await _ipbApiHelper.CreateOrUpdateNewsTopic(command.News);
            if (result.topicId > 0 && result.postId > 0)
            {
                command.News.ForumTopicId = result.topicId;
                command.News.ForumPostId = result.postId;
                DBContext.Update(command.News);
                await DBContext.SaveChangesAsync();
            }
        }
    }
}