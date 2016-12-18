using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class ParentGalleryViewModel : BaseViewModel
    {
        public ParentGalleryViewModel(BaseViewModelConfig config, ParentModel parent,
            List<CatsTree<GalleryCat, GalleryPic>> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
            Title = $"{Parent.DisplayTitle} - Галерея";
        }

        public List<CatsTree<GalleryCat, GalleryPic>> Cats;
        public ParentModel Parent;

        public string ParentGalleryUrl => UrlManager.Gallery.ParentGalleryUrl((dynamic)Parent);
        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic)Parent);
    }
}