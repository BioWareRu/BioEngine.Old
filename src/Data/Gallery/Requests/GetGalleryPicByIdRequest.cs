using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Requests
{
    public class GetGalleryPicByIdRequest : RequestBase<GalleryPic>
    {
        public GetGalleryPicByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}