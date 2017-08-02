using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class GalleryCatViewModel : BaseViewModel
    {
        public GalleryCatViewModel(BaseViewModelConfig config, GalleryCat cat,
            IEnumerable<CatsTree<GalleryCat, GalleryPic>> children,
            IEnumerable<GalleryPic> pics, int totalPictures, int currentPage) : base(config)
        {
            GalleryCat = cat;
            Children = children;
            Pictures = pics;

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
            return Task.FromResult($"Картинки категории \"{GalleryCat.Title}\" в разделе \"{GalleryCat.Parent?.DisplayTitle}\"");
        }

        public GalleryCat GalleryCat { get; }

        public IEnumerable<CatsTree<GalleryCat, GalleryPic>> Children { get; }

        public IEnumerable<GalleryPic> Pictures { get; }

        public int CurrentPage { get; }
        public int TotalPictures { get; }
    }
}