using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Notifications
{
    public class FileDownloadedNotification : NotificationBase
    {
        public FileDownloadedNotification(File file)
        {
            File = file;
        }

        public File File { get; }
    }
}