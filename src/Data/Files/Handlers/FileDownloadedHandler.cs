using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FileDownloadedHandler : CommandHandlerBase<FileDownloadedCommand>
    {
        public FileDownloadedHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task Handle(FileDownloadedCommand command)
        {
            command.File.Count++;
            DBContext.Files.Update(command.File);
            await DBContext.SaveChangesAsync();
        }
    }
}