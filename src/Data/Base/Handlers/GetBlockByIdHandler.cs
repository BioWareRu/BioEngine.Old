using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    public class GetBlockByIdHandler : RequestHandlerBase<GetBlockByIdRequest, Block>
    {
        public GetBlockByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Block> Handle(GetBlockByIdRequest message)
        {
            return await DBContext.Blocks.FirstOrDefaultAsync(x => x.Id == "counter");
        }
    }
}