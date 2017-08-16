using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetMenuByKeyHandler : QueryHandlerBase<GetMenuByKeyQuery, Menu>
    {
        public GetMenuByKeyHandler(HandlerContext<GetMenuByKeyHandler> context) : base(context)
        {
        }

        protected override async Task<Menu> RunQuery(GetMenuByKeyQuery message)
        {
            return await DBContext.Menus.Where(x => x.Key == message.Key).FirstOrDefaultAsync();
        }
    }
}