using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Requests
{
    public class GalleryCategoryProcessRequest : CategoryProcessRequestBase<GalleryCat>
    {
        public GalleryCategoryProcessRequest(GalleryCat cat, ICategoryRequest<GalleryCat> request) : base(cat, request)
        {
        }
    }
}