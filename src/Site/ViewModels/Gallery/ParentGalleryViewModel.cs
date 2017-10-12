using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Gallery
{
    public class ParentGalleryViewModel : BaseViewModel
    {
        public readonly IEnumerable<GalleryCat> Cats;
        public readonly IParentModel Parent;

        public ParentGalleryViewModel(BaseViewModelConfig config, IParentModel parent,
            IEnumerable<GalleryCat> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
        }

        public override string Title()
        {
            return $"{Parent.DisplayTitle} - Галерея";
        }

        protected override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Картинки в разделе «{Parent.DisplayTitle}»");
        }
    }
}