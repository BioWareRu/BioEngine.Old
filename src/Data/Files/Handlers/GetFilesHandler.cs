using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFilesHandler : QueryHandlerBase<GetFilesQuery, (IEnumerable<Common.Models.File>
        files, int count)>
    {
        public GetFilesHandler(IMediator mediator, BWContext dbContext, ILogger<GetFilesHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Common.Models.File> files, int count)> RunQuery(
            GetFilesQuery message)
        {
            var query = DBContext.Files.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }
            var totalFiles = await query.CountAsync();

            if (message.Page != null && message.Page > 0)
            {
                query = query.Skip(((int) message.Page - 1) * message.PageSize)
                    .Take(message.PageSize);
            }

            var files =
                await query
                    .OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Cat)
                    .ToListAsync();

            foreach (var file in files)
            {
                file.Cat =
                    await Mediator.Send(new FileCategoryProcessQuery(file.Cat,
                        new GetFilesCategoryQuery(message.Parent)));
            }

            return (files, totalFiles);
        }
    }
}