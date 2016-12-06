using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileViewModel : BaseViewModel
    {
        public File File { get; }
        public UrlManager UrlManager { get; set; }

        public FileViewModel(IEnumerable<Settings> settings, File file, UrlManager urlManager) : base(settings)
        {
            File = file;
            UrlManager = urlManager;
            var title = file.Title;
            if (file.Cat != null)
            {
                title += " - " + file.Cat.Title;
            }
            if (file.Parent != null)
            {
                title += " - " + file.Parent.DisplayTitle;
            }
            Title = title;
        }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(File.Date);

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) File.Parent);
        public string ParentFilesUrl => UrlManager.Files.ParentFilesUrl((dynamic) File.Parent);
    }
}