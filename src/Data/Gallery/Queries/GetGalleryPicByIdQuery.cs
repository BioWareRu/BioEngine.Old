using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GetGalleryPicByIdQuery : SingleModelQueryBase<GalleryPic>
    {
        public GetGalleryPicByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}