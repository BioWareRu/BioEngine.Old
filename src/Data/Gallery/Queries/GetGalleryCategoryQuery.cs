using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GetGalleryCategoryQuery : SingleModelQueryBase<GalleryCat>, ICategoryQuery<GalleryCat>
    {
        public IParentModel Parent { get; set; }
        public bool LoadChildren { get; set; }
        public GalleryCat ParentCat { get; set; }
        public int? LoadLastItems { get; set; }
        public string Url { get; set; }
    }
}