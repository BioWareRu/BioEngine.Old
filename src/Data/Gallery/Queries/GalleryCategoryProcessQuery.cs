using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GalleryCategoryProcessQuery : CategoryProcessQueryBase<GalleryCat>
    {
        public GalleryCategoryProcessQuery(GalleryCat cat, ICategoryQuery<GalleryCat> query) : base(cat, query)
        {
        }
    }
}