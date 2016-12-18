using System.Collections.Generic;
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
            var title = GalleryCat.Title;
            title += " - Галерея";
            if (GalleryCat.Parent != null)
                title += " - " + GalleryCat.Parent.DisplayTitle;
            Title = title;
            TotalPictures = totalPictures;
            CurrentPage = currentPage;
        }

        public GalleryCat GalleryCat { get; }

        public IEnumerable<CatsTree<GalleryCat, GalleryPic>> Children { get; }

        public IEnumerable<GalleryPic> Pictures { get; }

        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) GalleryCat.Parent);
        public string ParentGalleryUrl => UrlManager.Gallery.ParentGalleryUrl((dynamic) GalleryCat.Parent);
        public int CurrentPage { get; }
        public int TotalPictures { get; }
    }
}