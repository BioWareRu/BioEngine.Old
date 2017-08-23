using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetDeveloperByIdHandler : QueryHandlerBase<GetDeveloperByIdQuery, Developer>
    {
        public GetDeveloperByIdHandler(HandlerContext<GetDeveloperByIdHandler> context) : base(context)
        {
        }

        protected override async Task<Developer> RunQueryAsync(GetDeveloperByIdQuery message)
        {
            return await DBContext.Developers.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}