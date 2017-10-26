using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GetGalleryCategoryByIdQuery : SingleModelQueryBase<GalleryCat>, ICategoryQuery<GalleryCat>
    {
        public GetGalleryCategoryByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public GalleryCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}