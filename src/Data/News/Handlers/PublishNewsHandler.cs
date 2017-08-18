using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;
using Social;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class PublishNewsHandler : RestCommandHandlerBase<PublishNewsCommand, bool>
    {
        public PublishNewsHandler(HandlerContext<PublishNewsHandler> context,
            IValidator<PublishNewsCommand>[] validators) : base(context,
            validators)
        {
        }

        protected override async Task<bool> ExecuteCommand(PublishNewsCommand command)
        {
            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.Model.Pub = 1;

            await Mediator.Send(new ManageNewsTweetCommand(command.Model, TwitterOperationEnum.CreateOrUpdate));

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}