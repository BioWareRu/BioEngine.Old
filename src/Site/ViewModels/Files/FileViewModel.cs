using System;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileViewModel : BaseViewModel
    {
        public FileViewModel(BaseViewModelConfig config, File file) : base(config)
        {
            File = file;
            var title = file.Title;
            if (file.Cat != null)
                title += " - " + file.Cat.Title;
            if (file.Parent != null)
                title += " - " + file.Parent.DisplayTitle;
            Title = title;
        }

        public File File { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(File.Date);

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) File.Parent);
        public string ParentFilesUrl => UrlManager.Files.ParentFilesUrl((dynamic) File.Parent);
    }
}