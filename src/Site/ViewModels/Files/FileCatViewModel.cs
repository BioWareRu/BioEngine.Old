using System.Collections.Generic;
using System.Threading.Tasks;
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
        }

        public FileCat FileCat { get; }

        public override string Title()
        {
            var title = FileCat.Title;
            title += " - Файлы";
            if (FileCat.Parent != null)
                title += " - " + FileCat.Parent.DisplayTitle;

            return title;
        }

        protected override Task<string> GetDescription()
        {
            return Task.FromResult($"Статьи категории \"{FileCat.Title}\" в разделе \"{FileCat.Parent?.DisplayTitle}\"");
        }

        public IEnumerable<CatsTree<FileCat, File>> Children { get; }

        public IEnumerable<File> LastFiles { get; }

        public int CurrentPage { get; }
        public int TotalFiles { get; }
    }
}