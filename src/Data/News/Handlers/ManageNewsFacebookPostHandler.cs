using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Data.Social.Commands;
using BioEngine.Social;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class ManageNewsFacebookPostHandler : QueryHandlerBase<ManageNewsFacebookCommand, bool>
    {
        public ManageNewsFacebookPostHandler(HandlerContext<ManageNewsFacebookPostHandler> context) : base(context)
        {
        }

        protected override async Task<bool> RunQueryAsync(ManageNewsFacebookCommand message)
        {
            var result = false;
            switch (message.Operation)
            {
                case FacebookOperationEnum.CreateOrUpdate:
                    if (!string.IsNullOrEmpty(message.News.FacebookId))
                    {
                        var deleted = await Mediator.Send(new DeleteFacebookPostCommand(message.News.FacebookId));
                        if (!deleted)
                        {
                            throw new Exception("Can't delete news facebook post");
                        }
                    }

                    var newPostId = await Mediator.Send(new PublishFacebookLinkCommand(message.News.PublicUrl));
                    if (!string.IsNullOrEmpty(newPostId))
                    {
                        message.News.FacebookId = newPostId;
                        result = true;
                    }

                    break;
                case FacebookOperationEnum.Delete:
                    if (!string.IsNullOrEmpty(message.News.FacebookId))
                    {
                        result = await Mediator.Send(new DeleteFacebookPostCommand(message.News.FacebookId));
                        if (result)
                        {
                            message.News.FacebookId = null;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}