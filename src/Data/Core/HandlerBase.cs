using BioEngine.Common.DB;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class HandlerBase
    {
        protected readonly BWContext DBContext;
        protected readonly ILogger Logger;
        protected readonly IMediator Mediator;

        protected HandlerBase(IMediator mediator, BWContext dbContext, ILogger logger)
        {
            DBContext = dbContext;
            Logger = logger;
            Mediator = mediator;
        }
    }
}
