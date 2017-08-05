using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Ipb;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class DeleteNewsForumTopicHandler : CommandHandlerBase<DeleteNewsForumTopicCommand>
    {
        private readonly IPBApiHelper _ipbApiHelper;

        public DeleteNewsForumTopicHandler(IMediator mediator, BWContext dbContext,
            ILogger<DeleteNewsForumTopicHandler> logger,
            IPBApiHelper ipbApiHelper) : base(mediator, dbContext, logger)
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