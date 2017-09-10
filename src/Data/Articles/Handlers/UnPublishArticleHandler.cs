using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class UnPublishArticleHandler : RestCommandHandlerBase<UnPublishArticleCommand, bool>
    {
        public UnPublishArticleHandler(HandlerContext<UnPublishArticleHandler> context,
            IValidator<UnPublishArticleCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UnPublishArticleCommand command)
        {
            command.Model.Pub = 0;

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.Article>(command.Model));

            return true;
        }
    }
}