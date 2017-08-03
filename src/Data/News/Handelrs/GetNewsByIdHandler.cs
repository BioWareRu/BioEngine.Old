using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.News.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.News.Handelrs
{
    public class GetNewsByIdHandler : RequestHandlerBase<GetNewsByIdRequest, Common.Models.News>
    {
        public GetNewsByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Common.Models.News> Handle(GetNewsByIdRequest message)
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