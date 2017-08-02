using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Requests
{
    public class GetGalleryPicsRequest : RequestBase<(IEnumerable<Common.Models.GalleryPic> pics, int count)>
    {
        public bool WithUnPublishedPictures { get; }
        public int Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public GetGalleryPicsRequest(bool withUnPublishedPictures = false, int page = 1, IParentModel parent = null)
        {
            WithUnPublishedPictures = withUnPublishedPictures;
            Page = page;
            Parent = parent;
        }
    }
}