using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.News.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handelrs
{
    [UsedImplicitly]
    internal class GetNewsHandler : QueryHandlerBase<GetNewsQuery, (IEnumerable<Common.Models.News> news, int count)>
    {
        public GetNewsHandler(IMediator mediator, BWContext dbContext, ILogger<GetNewsHandler> logger) : base(mediator,
            dbContext, logger)
        {
        }

        protected override async Task<(IEnumerable<Common.Models.News> news, int count)> RunQuery(GetNewsQuery message)
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

            var totalNews = await query.CountAsync();

            query = query
                .OrderByDescending(x => x.Sticky)
                .ThenByDescending(x => x.Date)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic);

            if (message.Page != null && message.Page > 0)
            {
                query = query.Skip(((int) message.Page - 1) * message.PageSize)
                    .Take(message.PageSize);
            }

            var news = await query.ToListAsync();


            return (news, totalNews);
        }
    }
}