using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class PublishArticleHandler : RestCommandHandlerBase<PublishArticleCommand, bool>
    {
        public PublishArticleHandler(HandlerContext<PublishArticleHandler> context,
            IValidator<PublishArticleCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(PublishArticleCommand command)
        {
            command.Model.Pub = 1;

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new IndexEntityCommand<Common.Models.Article>(command.Model));

            return true;
        }
    }
}