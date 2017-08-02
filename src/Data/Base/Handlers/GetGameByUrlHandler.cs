using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    public class GetGameByUrlHandler : RequestHandlerBase<GetGameByUrlRequest, Game>
    {
        public GetGameByUrlHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Game> Handle(GetGameByUrlRequest message)
        {
            return await DBContext.Games.Include(x => x.Developer).FirstOrDefaultAsync(x => x.Url == message.Url);
        }
    }
}