using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FileDownloadedHandler : CommandHandlerBase<FileDownloadedCommand>
    {
        public FileDownloadedHandler(HandlerContext<FileDownloadedHandler> context) : base(context)
        {
        }

        protected override async Task ExecuteCommandAsync(FileDownloadedCommand command)
        {
            command.File.Count++;
            DBContext.Files.Update(command.File);
            await DBContext.SaveChangesAsync();
        }
    }
}