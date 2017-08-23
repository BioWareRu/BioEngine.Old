using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    [UsedImplicitly]
    internal class GetTopicByIdHandler : QueryHandlerBase<GetTopicByIdQuery, Topic>
    {
        public GetTopicByIdHandler(HandlerContext<GetTopicByIdHandler> context) : base(context)
        {
        }

        protected override async Task<Topic> RunQueryAsync(GetTopicByIdQuery message)
        {
            return await DBContext.Topics.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}