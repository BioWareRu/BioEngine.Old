using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    public class GetGamesHandler : RequestHandlerBase<GetGamesRequest, IEnumerable<Game>>
    {
        public GetGamesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<IEnumerable<Game>> Handle(GetGamesRequest message)
        {
            return await DBContext.Games.ToListAsync();
        }
    }
}