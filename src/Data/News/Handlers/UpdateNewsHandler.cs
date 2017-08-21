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
    internal class UpdateNewsHandler : RestCommandHandlerBase<UpdateNewsCommand, bool>
    {
        private readonly ISearchProvider<Common.Models.News> _newsSearchProvider;


        public UpdateNewsHandler(HandlerContext<UpdateNewsHandler> context, IValidator<UpdateNewsCommand>[] validators,
            ISearchProvider<Common.Models.News> newsSearchProvider)
            : base(context, validators)
        {
            _newsSearchProvider = newsSearchProvider;
        }

        protected override async Task<bool> ExecuteCommand(UpdateNewsCommand command)
        {
            command.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();

            await Validate(command);

            var needTweetUpd = command.Model.Pub == 1 &&
                               (command.Title != command.Model.Title || command.Url != command.Model.Url);

            Mapper.Map(command, command.Model);
            if (needTweetUpd)
            {
                await Mediator.Send(new ManageNewsTweetCommand(command.Model, TwitterOperationEnum.CreateOrUpdate));
            }

            await Mediator.Publish(new CreateOrUpdateNewsForumTopicCommand(command.Model));

            await _newsSearchProvider.AddUpdateEntity(command.Model);

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}