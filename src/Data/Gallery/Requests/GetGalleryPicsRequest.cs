using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Requests
{
    public class GetGalleryPicsRequest : RequestBase<(IEnumerable<GalleryPic> pics, int count)>
    {
        public bool WithUnPublishedPictures { get; }
        public int? Page { get; }
        public IParentModel Parent { get; }
        public GalleryCat Cat { get; }

        public int PageSize { get; set; } = 20;

        public bool LoadPicPositions { get; set; } = false;

        public GetGalleryPicsRequest(bool withUnPublishedPictures = false, int? page = 1, IParentModel parent = null,
            GalleryCat cat = null)
        {
            WithUnPublishedPictures = withUnPublishedPictures;
            Page = page;
            Parent = parent;
            Cat = cat;
        }
    }
}