using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetCategoryFilesHandler : ModelListQueryHandlerBase<GetCategoryFilesQuery, File>
    {
        public GetCategoryFilesHandler(IMediator mediator, BWContext dbContext, ILogger<GetCategoryFilesHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<File>, int)> RunQuery(GetCategoryFilesQuery message)
        {
            var filesQuery = DBContext.Files.Where(x => x.CatId == message.Cat.Id);

            return await GetData(filesQuery, message);
        }
    }
}