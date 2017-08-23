using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Data.Search.Commands;
using BioEngine.Social;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class UpdateNewsHandler : RestCommandHandlerBase<UpdateNewsCommand, bool>
    {
        public UpdateNewsHandler(HandlerContext<UpdateNewsHandler> context, IValidator<UpdateNewsCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UpdateNewsCommand command)
        {
            var needTweetUpd = command.Model.Pub == 1 &&
                               (command.Title != command.Model.Title || command.Url != command.Model.Url);

            Mapper.Map(command, command.Model);
            if (needTweetUpd)
            {
                await Mediator.Send(new ManageNewsTweetCommand(command.Model, TwitterOperationEnum.CreateOrUpdate));
            }

            await Mediator.Publish(new CreateOrUpdateNewsForumTopicCommand(command.Model));

            if (command.Model.Pub == 1)
            {
                await Mediator.Publish(new IndexEntityCommand<Common.Models.News>(command.Model));
            }
            
            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}