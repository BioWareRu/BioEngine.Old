using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Commands
{
    public sealed class DeleteFileCatCommand : UpdateCommand<Common.Models.FileCat>
    {
        public DeleteFileCatCommand(Common.Models.FileCat fileCat)
        {
            Model = fileCat;
        }
    }
}