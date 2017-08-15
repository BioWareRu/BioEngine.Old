using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class GetNewsHandler : ModelListQueryHandlerBase<GetNewsQuery, Common.Models.News>
    {
        private readonly BioUrlManager _urlManager;

        public GetNewsHandler(IMediator mediator, BWContext dbContext, ILogger<GetNewsHandler> logger, BioUrlManager urlManager) : base(mediator,
            dbContext, logger)
        {
            _urlManager = urlManager;
        }

        protected override async Task<(IEnumerable<Common.Models.News>, int)> RunQuery(GetNewsQuery message)
        {
            var query = DBContext.News.AsQueryable();
            if (!message.WithUnPublishedNews)
                query = query.Where(x => x.Pub == 1);
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }
            if (message.DateStart != null)
            {
                query = query.Where(x => x.Date >= message.DateStart);
            }
            if (message.DateEnd != null)
            {
                query = query.Where(x => x.Date <= message.DateEnd);
            }

            query = query
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic);

            var data = await GetData(query, message);
            foreach (var newse in data.models)
            {
                newse.PublicUrl = _urlManager.News.PublicUrl(newse, true    );
            }
            return data;
        }
    }
}