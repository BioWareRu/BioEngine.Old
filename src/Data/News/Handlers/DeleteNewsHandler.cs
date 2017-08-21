using System.Threading.Tasks;
using BioEngine.Common.Search;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class DeleteNewsHandler : RestCommandHandlerBase<DeleteNewsCommand, bool>
    {
        private readonly ISearchProvider<Common.Models.News> _newsSearchProvider;

        public DeleteNewsHandler(HandlerContext<DeleteNewsHandler> context, IValidator<DeleteNewsCommand>[] validators,
            ISearchProvider<Common.Models.News> newsSearchProvider)
            : base(context,
                validators)
        {
            _newsSearchProvider = newsSearchProvider;
        }

        protected override async Task<bool> ExecuteCommand(DeleteNewsCommand command)
        {
            //first - delete forum topic
            await Mediator.Publish(new DeleteNewsForumTopicCommand(command.Model));

            //TODO: delete twitter

            //delete news
            DBContext.Remove(command.Model);
            await DBContext.SaveChangesAsync();

            await _newsSearchProvider.DeleteEntity(command.Model);

            return true;
        }
    }
}