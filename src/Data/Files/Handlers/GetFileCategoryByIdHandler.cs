using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFileCategoryByIdHandler : QueryHandlerBase<GetFileCategoryByIdQuery, FileCat>
    {
        public GetFileCategoryByIdHandler(HandlerContext<GetFileCategoryByIdQuery> context) : base(context)
        {
        }

        protected override async Task<FileCat> RunQueryAsync(GetFileCategoryByIdQuery message)
        {
            var cat = await DBContext.FileCats
                .Where(x => x.Id == message.Id)
                .Include(x => x.ParentCat)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .FirstOrDefaultAsync();
            if (cat != null)
            {
                cat = await Mediator.Send(new FileCategoryProcessQuery(cat, message));
            }
            return cat;
        }
    }
}