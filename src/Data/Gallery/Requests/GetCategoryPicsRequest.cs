using System.Collections.Generic;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Requests
{
    public class GetCategoryPicsRequest : RequestBase<(IEnumerable<GalleryPic> pics, int count)>
    {
        public GetCategoryPicsRequest(GalleryCat cat, int page = 1)
        {
            Cat = cat;
            Page = page;
        }

        public int PageSize { get; set; } = 20;

        public GalleryCat Cat { get; }
        public int Page { get; }
    }
}