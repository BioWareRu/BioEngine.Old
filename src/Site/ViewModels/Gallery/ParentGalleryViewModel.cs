using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class ParentGalleryViewModel : BaseViewModel
    {
        public readonly List<CatsTree<GalleryCat, GalleryPic>> Cats;
        public readonly IParentModel Parent;

        public ParentGalleryViewModel(BaseViewModelConfig config, IParentModel parent,
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

        public override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Картинки в разделе \"{Parent.DisplayTitle}\"");
        }
    }
}