using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Handlers;
using BioEngine.Data.News.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handelrs
{
    [UsedImplicitly]
    internal class GetNewsByIdHandler : QueryHandlerBase<GetNewsByIdQuery, Common.Models.News>
    {
        public GetNewsByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetNewsByIdHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<Common.Models.News> RunQuery(GetNewsByIdQuery message)
        {
            var news =
                await DBContext.News
                    .Where(x => x.Id == message.Id)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .FirstOrDefaultAsync();

            return news;
        }
    }
}