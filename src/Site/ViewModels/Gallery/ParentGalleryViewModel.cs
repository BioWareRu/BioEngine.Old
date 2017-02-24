using System.Collections.Generic;
using System.Threading.Tasks;
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
        }

        public override Task<string> Title()
        {
            return Task.FromResult($"{Parent.DisplayTitle} - Галерея");
        }
    }
}