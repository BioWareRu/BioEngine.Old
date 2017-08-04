using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class
        GalleryCategoryProcessHandler : CategoryProcessHandlerBase<GalleryCategoryProcessQuery, GalleryCat, GalleryPic
        >
    {
        public GalleryCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<GalleryPic>> GetCatItems(GalleryCat cat, int count)
        {
            return (await Mediator.Send(new GetGalleryPicsQuery(cat: cat) {PageSize = count})).pics;
        }
    }
}