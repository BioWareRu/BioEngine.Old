using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetCategoryFilesHandler : QueryHandlerBase<GetCategoryFilesQuery, (IEnumerable<File>
        files, int count)>
    {
        public GetCategoryFilesHandler(IMediator mediator, BWContext dbContext, ILogger<GetCategoryFilesHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<File> files, int count)> RunQuery(
            GetCategoryFilesQuery message)
        {
            var filesQuery = DBContext.Files.Where(x => x.CatId == message.Cat.Id)
                .OrderByDescending(x => x.Id).AsQueryable();

            var count = await filesQuery.CountAsync();

            if (message.Page > 0)
            {
                filesQuery = filesQuery.Skip((message.Page - 1) * message.PageSize).Take(message.PageSize);
            }

            var files = await filesQuery.ToListAsync();

            return (files, count);
        }
    }
}