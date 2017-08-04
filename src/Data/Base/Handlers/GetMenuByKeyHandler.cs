using System.Linq;
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
    internal class GetMenuByKeyHandler : QueryHandlerBase<GetMenuByKeyQuery, Menu>
    {
        public GetMenuByKeyHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Menu> Handle(GetMenuByKeyQuery message)
        {
            return await DBContext.Menus.Where(x => x.Key == message.Key).FirstOrDefaultAsync();
        }
    }
}