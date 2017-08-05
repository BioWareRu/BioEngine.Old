using System.Linq;
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
    internal class GetMenuByKeyHandler : QueryHandlerBase<GetMenuByKeyQuery, Menu>
    {
        public GetMenuByKeyHandler(IMediator mediator, BWContext dbContext, ILogger<GetMenuByKeyHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<Menu> RunQuery(GetMenuByKeyQuery message)
        {
            return await DBContext.Menus.Where(x => x.Key == message.Key).FirstOrDefaultAsync();
        }
    }
}