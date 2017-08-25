using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Data.Search.Commands;
using BioEngine.Data.Social.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class DeleteNewsHandler : RestCommandHandlerBase<DeleteNewsCommand, bool>
    {
        public DeleteNewsHandler(HandlerContext<DeleteNewsHandler> context, IValidator<DeleteNewsCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(DeleteNewsCommand command)
        {
            //first - delete forum topic
            await Mediator.Publish(new DeleteNewsForumTopicCommand(command.Model));

            if (command.Model.TwitterId > 0)
            {
                await Mediator.Send(new DeleteTweetCommand(command.Model.TwitterId));
            }

            //delete news
            DBContext.Remove(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.News>(command.Model));

            return true;
        }
    }
}