using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class GalleryCatViewModel : BaseViewModel
    {
        public GalleryCatViewModel(BaseViewModelConfig config, GalleryCat cat, int totalPictures,
            int currentPage) : base(config)
        {
            GalleryCat = cat;
            TotalPictures = totalPictures;
            CurrentPage = currentPage;
        }

        public override string Title()
        {
            var title = GalleryCat.Title;
            title += " - Галерея";
            if (GalleryCat.Parent != null)
                title += " - " + GalleryCat.Parent.DisplayTitle;
            return title;
        }

        protected override Task<string> GetDescription()
        {
            return Task.FromResult(
                $"Картинки категории «{GalleryCat.Title}» в разделе «{GalleryCat.Parent?.DisplayTitle}»");
        }

        public GalleryCat GalleryCat { get; }

        public int CurrentPage { get; }
        public int TotalPictures { get; }
    }
}