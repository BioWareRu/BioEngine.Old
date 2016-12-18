using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class ParentGalleryViewModel : BaseViewModel
    {
        public readonly List<CatsTree<GalleryCat, GalleryPic>> Cats;
        public readonly ParentModel Parent;

        public ParentGalleryViewModel(BaseViewModelConfig config, ParentModel parent,
            List<CatsTree<GalleryCat, GalleryPic>> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
            Title = $"{Parent.DisplayTitle} - Галерея";
        }

        public string ParentGalleryUrl => UrlManager.Gallery.ParentGalleryUrl((dynamic) Parent);
        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) Parent);
    }
}