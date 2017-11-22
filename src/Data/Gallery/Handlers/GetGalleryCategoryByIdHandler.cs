using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery.Handlers
{
    [UsedImplicitly]
    internal class GetGalleryCategoryByIdHandler : QueryHandlerBase<GetGalleryCategoryByIdQuery, GalleryCat>
    {
        public GetGalleryCategoryByIdHandler(HandlerContext<GetGalleryCategoryByIdQuery> context) : base(context)
        {
        }

        protected override async Task<GalleryCat> RunQueryAsync(GetGalleryCategoryByIdQuery message)
        {
            return await Repository.Gallery.GetCatById(message.Id);
        }
    }
}