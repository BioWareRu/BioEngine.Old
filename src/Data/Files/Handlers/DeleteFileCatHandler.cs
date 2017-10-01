using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class DeleteFileCatHandler : RestCommandHandlerBase<DeleteFileCatCommand, bool>
    {
        public DeleteFileCatHandler(HandlerContext<DeleteFileCatHandler> context,
            IValidator<DeleteFileCatCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(DeleteFileCatCommand command)
        {
            DBContext.Remove(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.FileCat>(command.Model));

            return true;
        }
    }
}