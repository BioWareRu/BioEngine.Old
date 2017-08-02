using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.News.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.News.Handelrs
{
    public class GetNewsHandler : RequestHandlerBase<GetNewsRequest, (IEnumerable<Common.Models.News> news, int count)>
    {
        public GetNewsHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<(IEnumerable<Common.Models.News> news, int count)> Handle(GetNewsRequest message)
        {
            var query = DBContext.News.AsQueryable();
            if (!message.WithUnPublishedNews)
                query = query.Where(x => x.Pub == 1);
            var news =
                await query
                    .OrderByDescending(x => x.Sticky)
                    .ThenByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Skip((message.Page - 1) * 20)
                    .Take(20)
                    .ToListAsync();
            var totalNews = await DBContext.News.CountAsync();

            return (news, totalNews);
        }
    }
}