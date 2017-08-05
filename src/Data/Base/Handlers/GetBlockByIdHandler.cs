using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetBlockByIdHandler : QueryHandlerBase<GetBlockByIdQuery, Block>
    {
        public GetBlockByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetBlockByIdHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<Block> RunQuery(GetBlockByIdQuery message)
        {
            return await DBContext.Blocks.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}