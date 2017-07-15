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

        public override async Task<string> Title()
        {
            var parent = await ParentEntityProvider.GetModelParent(GalleryCat);
            var title = GalleryCat.Title;
            title += " - Галерея";
            if (parent != null)
                title += " - " + parent.DisplayTitle;
            return title;
        }

        public override async Task<string> GetDescription()
        {
            var parent = await ParentEntityProvider.GetModelParent(GalleryCat);
            return $"Картинки категории \"{GalleryCat.Title}\" в разделе \"{parent?.DisplayTitle}\"";
        }

        public GalleryCat GalleryCat { get; }

        public IEnumerable<CatsTree<GalleryCat, GalleryPic>> Children { get; }

        public IEnumerable<GalleryPic> Pictures { get; }

        public int CurrentPage { get; }
        public int TotalPictures { get; }
    }
}