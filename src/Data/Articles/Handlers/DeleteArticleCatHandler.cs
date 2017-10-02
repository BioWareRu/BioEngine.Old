using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class DeleteArticleCatHandler : RestCommandHandlerBase<DeleteArticleCatCommand, bool>
    {
        public DeleteArticleCatHandler(HandlerContext<DeleteArticleCatHandler> context,
            IValidator<DeleteArticleCatCommand>[] validators) 
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(DeleteArticleCatCommand command)
        {
            DBContext.Remove(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.ArticleCat>(command.Model));

            return true;
        }
    }
}