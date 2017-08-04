using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Commands
{
    public class FileDownloadedCommand : CommandBase
    {
        public FileDownloadedCommand(File file)
        {
            File = file;
        }

        public File File { get; }
    }
}