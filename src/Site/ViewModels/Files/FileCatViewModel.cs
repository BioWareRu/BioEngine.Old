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

        public override async Task<string> Title()
        {
            var title = FileCat.Title;
            title += " - Файлы";
            var parent = await ParentEntityProvider.GetModelParent(FileCat);
            if (parent != null)
                title += " - " + parent.DisplayTitle;

            return title;
        }

        protected override async Task<string> GetDescription()
        {
            var parent = await ParentEntityProvider.GetModelParent(FileCat);
            return $"Статьи категории \"{FileCat.Title}\" в разделе \"{parent?.DisplayTitle}\"";
        }

        public IEnumerable<CatsTree<FileCat, File>> Children { get; }

        public IEnumerable<File> LastFiles { get; }

        public int CurrentPage { get; }
        public int TotalFiles { get; }
    }
}