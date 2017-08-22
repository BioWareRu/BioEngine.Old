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
    internal class ManageNewsTweetHandler : QueryHandlerBase<ManageNewsTweetCommand, bool>
    {
        public ManageNewsTweetHandler(HandlerContext<ManageNewsTweetHandler> context) : base(context)
        {
        }

        protected override async Task<bool> RunQuery(ManageNewsTweetCommand message)
        {
            var result = false;
            switch (message.Operation)
            {
                case TwitterOperationEnum.CreateOrUpdate:
                    if (message.News.TwitterId > 0)
                    {
                        var deleted = await Mediator.Send(new DeleteTweetCommand(message.News.TwitterId));
                        if (!deleted)
                        {
                            throw new Exception("Can't delete news tweet");
                        }
                    }
                    var text = $"{message.News.Title} {message.News.PublicUrl} #rpg";
                    if (!string.IsNullOrEmpty(message.News.Parent.TwitterTag))
                    {
                        text += $" {message.News.Parent.TwitterTag}";
                    }

                    var newTweetId = await Mediator.Send(new PublishTweetCommand(text));
                    if (newTweetId > 0)
                    {
                        message.News.TwitterId = newTweetId;
                        result = true;
                    }

                    break;
                case TwitterOperationEnum.Delete:
                    if (message.News.TwitterId > 0)
                    {
                        result = await Mediator.Send(new DeleteTweetCommand(message.News.TwitterId));
                        if (result)
                        {
                            message.News.TwitterId = 0;
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