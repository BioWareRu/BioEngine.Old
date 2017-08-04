using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetBlockByIdHandler : QueryHandlerBase<GetBlockByIdQuery, Block>
    {
        public GetBlockByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Block> Handle(GetBlockByIdQuery message)
        {
            return await DBContext.Blocks.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}