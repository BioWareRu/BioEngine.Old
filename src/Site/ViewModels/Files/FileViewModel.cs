using System;
using BioEngine.Common.Models;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.Files
{
    public class FileViewModel : BaseViewModel
    {
        public FileViewModel(BaseViewModelConfig config, File file) : base(config)
        {
            File = file;
        }

        public override async Task<string> Title()
        {
            var parent = await ParentEntityProvider.GetModelParent(File);
            var title = File.Title;
            if (File.Cat != null)
                title += " - " + File.Cat.Title;
            if (parent != null)
                title += " - " + parent.DisplayTitle;
            return title;
        }

        public File File { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(File.Date);
    }
}