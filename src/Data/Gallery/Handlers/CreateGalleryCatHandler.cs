using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Commands;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class CreateGalleryCatHandler : RestCommandHandlerBase<CreateGalleryCatCommand, int>
    {
        public CreateGalleryCatHandler(HandlerContext<CreateGalleryCatHandler> context,
            IValidator<CreateGalleryCatCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<int> ExecuteCommandAsync(CreateGalleryCatCommand command)
        {
            var galleryCat = Mapper.Map<CreateGalleryCatCommand, Common.Models.GalleryCat>(command);
            galleryCat = await Repository.Gallery.CreateCat(galleryCat);

            await Mediator.Publish(new IndexEntityCommand<Common.Models.GalleryCat>(galleryCat));

            return galleryCat.Id;
        }
    }
}