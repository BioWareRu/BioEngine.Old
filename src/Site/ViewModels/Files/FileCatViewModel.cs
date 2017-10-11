using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileCatViewModel : BaseViewModel
    {
        public FileCatViewModel(BaseViewModelConfig config, FileCat fileCat,int totalFiles,
            int currentPage)
            : base(config)
        {
            FileCat = fileCat;
            CurrentPage = currentPage;
            TotalFiles = totalFiles;
        }

        public int TotalFiles { get; }

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
            return Task.FromResult(
                $"Статьи категории «{FileCat.Title}» в разделе «{FileCat.Parent?.DisplayTitle}»");
        }

        public int CurrentPage { get; }
    }
}