using System;
using System.Linq;
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

        public override Func<IQueryable<GalleryPic>, IQueryable<GalleryPic>> OrderByFunc { get; protected set; } =
            pics => pics.OrderByDescending(x => x.Id);
    }
}