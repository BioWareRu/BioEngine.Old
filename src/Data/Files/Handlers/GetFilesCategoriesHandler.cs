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
    internal class GetFilesCategoriesHandler : ModelListQueryHandlerBase<GetFilesCategoriesQuery, FileCat>
    {
        public GetFilesCategoriesHandler(IMediator mediator, BWContext dbContext,
            ILogger<GetFilesCategoriesHandler> logger) : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<FileCat>, int)> RunQuery(GetFilesCategoriesQuery message)
        {
            var query = DBContext.FileCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.Pid == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.Pid == null);
            }

            var data = await GetData(query, message);

            foreach (var cat in data.models)
            {
                await Mediator.Send(new FileCategoryProcessQuery(cat, message));
            }

            return data;
        }
    }
}