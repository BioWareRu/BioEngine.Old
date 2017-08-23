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
    internal class PublishNewsHandler : RestCommandHandlerBase<PublishNewsCommand, bool>
    {
        public PublishNewsHandler(HandlerContext<PublishNewsHandler> context,
            IValidator<PublishNewsCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(PublishNewsCommand command)
        {
            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.Model.Pub = 1;

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Send(new ManageNewsTweetCommand(command.Model, TwitterOperationEnum.CreateOrUpdate));

            await Mediator.Publish(new IndexEntityCommand<Common.Models.News>(command.Model));

            return true;
        }
    }
}