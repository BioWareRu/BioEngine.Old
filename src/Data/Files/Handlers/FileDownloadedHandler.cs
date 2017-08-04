using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Notifications;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    public class FileDownloadedHandler : NotificationHandlerBase<FileDownloadedNotification>
    {
        public FileDownloadedHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task Handle(FileDownloadedNotification notification)
        {
            notification.File.Count++;
            DBContext.Files.Update(notification.File);
            await DBContext.SaveChangesAsync();
        }
    }
}