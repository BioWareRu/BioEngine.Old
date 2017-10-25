using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Commands;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class UpdateGalleryCatHandler : RestCommandHandlerBase<UpdateGalleryCatCommand, bool>
    {
        public UpdateGalleryCatHandler(HandlerContext<UpdateGalleryCatHandler> context,
            IValidator<UpdateGalleryCatCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UpdateGalleryCatCommand command)
        {
            Mapper.Map(command, command.Model);

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            await Mediator.Publish(new IndexEntityCommand<Common.Models.GalleryCat>(command.Model));

            return true;
        }
    }
}