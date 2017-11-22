using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Commands;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class DeleteArticleCatHandler : RestCommandHandlerBase<DeleteGalleryCatCommand, bool>
    {
        public DeleteArticleCatHandler(HandlerContext<DeleteArticleCatHandler> context,
            IValidator<DeleteGalleryCatCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(DeleteGalleryCatCommand command)
        {
            await Repository.Gallery.DeleteCat(command.Model);

            await Mediator.Publish(new DeleteEntityFromIndexCommand<Common.Models.GalleryCat>(command.Model));

            return true;
        }
    }
}