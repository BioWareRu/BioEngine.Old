using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFilesHandler : ModelListQueryHandlerBase<GetFilesQuery, File>
    {
        public GetFilesHandler(HandlerContext<GetFilesHandler> context) : base(context)
        {
        }

        protected override async Task<(IEnumerable<File>, int)> RunQueryAsync(GetFilesQuery message)
        {
            var query = DBContext.Files.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            query = query
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Cat);

            var data = await GetDataAsync(query, message);

            foreach (var file in data.models)
            {
                file.Cat =
                    await Mediator.Send(new FileCategoryProcessQuery(file.Cat,
                        new GetFilesCategoryQuery {Parent = message.Parent}));
            }

            return data;
        }
    }
}