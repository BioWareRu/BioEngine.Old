using System;
using System.Threading.Tasks;
using BioEngine.Common.Search;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using BioEngine.Social;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class PublishNewsHandler : RestCommandHandlerBase<PublishNewsCommand, bool>
    {
        private readonly ISearchProvider<Common.Models.News> _newsSearchProvider;

        public PublishNewsHandler(HandlerContext<PublishNewsHandler> context,
            IValidator<PublishNewsCommand>[] validators, ISearchProvider<Common.Models.News> newsSearchProvider) : base(
            context,
            validators)
        {
            _newsSearchProvider = newsSearchProvider;
        }

        protected override async Task<bool> ExecuteCommand(PublishNewsCommand command)
        {
            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.Model.Pub = 1;

            await Mediator.Send(new ManageNewsTweetCommand(command.Model, TwitterOperationEnum.CreateOrUpdate));

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            _newsSearchProvider.AddUpdateEntity(command.Model);

            return true;
        }
    }
}