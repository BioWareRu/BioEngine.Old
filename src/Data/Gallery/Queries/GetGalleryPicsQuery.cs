using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Gallery.Queries
{
    public class GetGalleryPicsQuery : ModelsListQueryBase<GalleryPic>
    {
        public bool WithUnPublishedPictures { get; set; }
        public IParentModel Parent { get; set; }
        public GalleryCat Cat { get; set; }

        public bool LoadPicPositions { get; set; }
    }
}