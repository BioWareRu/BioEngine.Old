using System.Linq;
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
    public class GetMenuByKeyHandler : RequestHandlerBase<GetMenuByKeyRequest, Menu>
    {
        public GetMenuByKeyHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<Menu> Handle(GetMenuByKeyRequest message)
        {
            return await DBContext.Menus.Where(x => x.Key == message.Key).FirstOrDefaultAsync();
        }
    }
}