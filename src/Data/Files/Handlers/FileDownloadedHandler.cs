using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FileDownloadedHandler : CommandHandlerBase<FileDownloadedCommand>
    {
        public FileDownloadedHandler(IMediator mediator, BWContext dbContext, ILogger<FileDownloadedHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task ExecuteCommand(FileDownloadedCommand command)
        {
            command.File.Count++;
            DBContext.Files.Update(command.File);
            await DBContext.SaveChangesAsync();
        }
    }
}