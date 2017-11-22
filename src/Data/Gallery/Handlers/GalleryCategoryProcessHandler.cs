using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GalleryCategoryProcessHandler :
        CategoryProcessHandlerBase<GalleryCategoryProcessQuery, GalleryCat, GalleryPic>
    {
        public GalleryCategoryProcessHandler(HandlerContext<GalleryCategoryProcessHandler> context,
            ParentEntityProvider parentEntityProvider) : base(context, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<GalleryPic>> GetCatItemsAsync(GalleryCat cat, int count)
        {
            return (await Repository.Gallery.GetPics(
                new GalleryPicsListQueryOptions {Cat = cat, Page = 1, PageSize = count})).models;
        }
    }
}