using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetCategoryFilesHandler : ModelListQueryHandlerBase<GetCategoryFilesQuery, File>
    {
        public GetCategoryFilesHandler(HandlerContext<GetCategoryFilesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<File>, int)> RunQueryAsync(GetCategoryFilesQuery message)
        {
            var filesQuery = DBContext.Files.Where(x => x.CatId == message.Cat.Id);

            return await GetDataAsync(filesQuery, message);
        }
    }
}