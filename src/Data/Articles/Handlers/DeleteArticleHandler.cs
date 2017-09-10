using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class DeleteArticleHandler : RestCommandHandlerBase<DeleteArticleCommand, bool>
    {
        public DeleteArticleHandler(HandlerContext<DeleteArticleHandler> context,
            IValidator<DeleteArticleCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(DeleteArticleCommand command)
        {
            DBContext.Remove(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.Article>(command.Model));

            return true;
        }
    }
}