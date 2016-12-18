using System.Collections.Generic;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileCatViewModel : BaseViewModel
    {
        public FileCatViewModel(BaseViewModelConfig config, FileCat fileCat,
            IEnumerable<CatsTree<FileCat, File>> children,
            IEnumerable<File> lastFiles, int currentPage, int totalFiles)
            : base(config)
        {
            FileCat = fileCat;
            Children = children;
            LastFiles = lastFiles;
            CurrentPage = currentPage;
            TotalFiles = totalFiles;
            var title = fileCat.Title;
            title += " - Файлы";
            if (fileCat.Parent != null)
                title += " - " + fileCat.Parent.DisplayTitle;
            Title = title;
        }

        public FileCat FileCat { get; }

        public IEnumerable<CatsTree<FileCat, File>> Children { get; }

        public IEnumerable<File> LastFiles { get; }

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) FileCat.Parent);
        public string ParentFilesUrl => UrlManager.Files.ParentFilesUrl((dynamic) FileCat.Parent);
        public int CurrentPage { get; }
        public int TotalFiles { get; }
    }
}