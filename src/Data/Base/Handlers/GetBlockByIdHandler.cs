using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetBlockByIdHandler : QueryHandlerBase<GetBlockByIdQuery, Block>
    {
        public GetBlockByIdHandler(HandlerContext<GetBlockByIdHandler> context) : base(context)
        {
        }

        protected override async Task<Block> RunQuery(GetBlockByIdQuery message)
        {
            return await DBContext.Blocks.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}