using AutoMapper;
using BioEngine.Common.DB;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    public abstract class HandlerContext
    {
        public IMediator Mediator { get; }
        public BWContext DBContext { get; }
        public BioRepository Repository { get; }
        public IMapper Mapper { get; }
        public ILogger Logger { get; }

        protected HandlerContext(IMediator mediator, BioRepository bioRepository, BWContext dbContext, IMapper mapper,
            ILogger logger)
        {
            Mediator = mediator;
            DBContext = dbContext;
            Logger = logger;
            Mapper = mapper;
            Repository = bioRepository;
        }
    }

    [UsedImplicitly]
    public class HandlerContext<T> : HandlerContext
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public HandlerContext(IMediator mediator, BioRepository bioRepository, BWContext dbContext, IMapper mapper,
            ILogger<T> logger) : base(
            mediator, bioRepository, dbContext, mapper, logger)
        {
        }
    }
}