using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileCatViewModel : BaseViewModel
    {
        public FileCat FileCat { get; }

        public IEnumerable<CatsTree<FileCat, File>> Children { get; }

        public IEnumerable<File> LastFiles { get; }

        public FileCatViewModel(BaseViewModelConfig config, FileCat fileCat,
            IEnumerable<CatsTree<FileCat, File>> children,
            IEnumerable<File> lastFiles)
            : base(config)
        {
            FileCat = fileCat;
            Children = children;
            LastFiles = lastFiles;
            var title = fileCat.Title;
            title += " - Файлы";
            if (fileCat.Parent != null)
            {
                title += " - " + fileCat.Parent.DisplayTitle;
            }
            Title = title;
        }

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) FileCat.Parent);
        public string ParentFilesUrl => UrlManager.Files.ParentFilesUrl((dynamic) FileCat.Parent);
        public int CurrentPage { get; set; }
        public int TotalFiles { get; set; }
    }
}