using System;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Common.DB;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class CreateNewsHandler : CommandWithReponseHandlerBase<CreateNewsCommand, Common.Models.News>
    {
        public CreateNewsHandler(IMediator mediator, BWContext dbContext,
            ILogger<CreateNewsHandler> logger,
            IValidator<CreateNewsCommand>[] validators, IMapper mapper) : base(mediator, dbContext, logger, validators,
            mapper)
        {
        }

        protected override async Task<Common.Models.News> ExecuteCommand(CreateNewsCommand command)
        {
            command.Date = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();

            await Validate(command);

            var news = Mapper.Map<CreateNewsCommand, Common.Models.News>(command);
            DBContext.News.Add(news);
            await DBContext.SaveChangesAsync();

            return news;
        }
    }
}