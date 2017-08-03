﻿using System.Collections.Generic;
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

            if (message.Page != null && message.Page > 0)
            {
                query = query.Skip(((int)message.Page - 1) * message.PageSize)
                    .Take(message.PageSize);
            }

            var news =
                await query
                    .OrderByDescending(x => x.Sticky)
                    .ThenByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .ToListAsync();


            return (news, totalNews);
        }
    }
}