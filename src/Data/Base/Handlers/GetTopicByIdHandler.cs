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
    internal class GetTopicByIdHandler : QueryHandlerBase<GetTopicByIdQuery, Topic>
    {
        public GetTopicByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetTopicByIdHandler> logger)
            : base(mediator, dbContext, logger)
        {
        }

        protected override async Task<Topic> RunQuery(GetTopicByIdQuery message)
        {
            return await DBContext.Topics.FirstOrDefaultAsync(x => x.Id == message.Id);
        }
    }
}