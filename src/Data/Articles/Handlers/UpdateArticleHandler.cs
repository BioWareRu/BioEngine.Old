using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class UpdateArticleHandler : RestCommandHandlerBase<UpdateArticleCommand, bool>
    {
        public UpdateArticleHandler(HandlerContext<UpdateArticleHandler> context,
            IValidator<UpdateArticleCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UpdateArticleCommand command)
        {
            Mapper.Map(command, command.Model);

            if (command.Model.Pub == 1)
            {
                await Mediator.Publish(new IndexEntityCommand<Common.Models.Article>(command.Model));
            }

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}