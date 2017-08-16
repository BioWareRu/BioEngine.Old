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
    internal class GetNewsByIdHandler : QueryHandlerBase<GetNewsByIdQuery, Common.Models.News>
    {
        private readonly BioUrlManager _urlManager;

        public GetNewsByIdHandler(HandlerContext<GetNewsByIdHandler> context,
            BioUrlManager urlManager) : base(context)
        {
            _urlManager = urlManager;
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
            if (news != null)
            {
                news.PublicUrl = _urlManager.News.PublicUrl(news, true);
            }
            return news;
        }
    }
}