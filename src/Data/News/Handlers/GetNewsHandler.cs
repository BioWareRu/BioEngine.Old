using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class GetNewsHandler : ModelListQueryHandlerBase<GetNewsQuery, Common.Models.News>
    {
        private readonly BioUrlManager _urlManager;

        public GetNewsHandler(HandlerContext<GetNewsHandler> context, BioUrlManager urlManager) : base(context)
        {
            _urlManager = urlManager;
        }

        protected override async Task<(IEnumerable<Common.Models.News>, int)> RunQueryAsync(GetNewsQuery message)
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

            try
            {
                var data = await GetDataAsync(query, message);
                foreach (var newse in data.models)
                {
                    newse.PublicUrl = _urlManager.News.PublicUrl(newse, true);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}