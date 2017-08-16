using AutoMapper;
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
        protected readonly IMapper Mapper;

        protected HandlerBase(HandlerContext context)
        {
            DBContext = context.DBContext;
            Logger = context.Logger;
            Mediator = context.Mediator;
            Mapper = context.Mapper;
        }
    }
}
