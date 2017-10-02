using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class UpdateArticleCatHandler : RestCommandHandlerBase<UpdateArticleCatCommand, bool>
    {
        public UpdateArticleCatHandler(HandlerContext<UpdateArticleCatCommand> context,
            IValidator<UpdateArticleCatCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UpdateArticleCatCommand command)
        {
            Mapper.Map(command, command.Model);

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}