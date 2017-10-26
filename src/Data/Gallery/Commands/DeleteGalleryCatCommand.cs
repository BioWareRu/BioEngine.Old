using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Commands
{
    public sealed class DeleteGalleryCatCommand : UpdateCommand<Common.Models.GalleryCat>
    {
        public DeleteGalleryCatCommand(Common.Models.GalleryCat galleryCat)
        {
            Model = galleryCat;
        }
    }
}