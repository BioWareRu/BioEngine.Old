using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GetGalleryCategoryQuery : QueryBase<GalleryCat>, ICategoryQuery<GalleryCat>
    {
        public GetGalleryCategoryQuery(IParentModel parent = null, GalleryCat parentCat = null,
            bool loadChildren = false,
            int? loadLastItems = null, string url = null)
        {
            Parent = parent;
            LoadChildren = loadChildren;
            ParentCat = parentCat;
            LoadLastItems = loadLastItems;
            Url = url;
        }

        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public GalleryCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}