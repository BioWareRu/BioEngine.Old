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
    internal class UpdateNewsHandler : CommandWithReponseHandlerBase<UpdateNewsCommand, bool>
    {
        public UpdateNewsHandler(IMediator mediator, BWContext dbContext, ILogger<UpdateNewsHandler> logger,
            IValidator<UpdateNewsCommand>[] validators, IMapper mapper) : base(mediator, dbContext, logger, validators,
            mapper)
        {
        }

        protected override async Task<bool> ExecuteCommand(UpdateNewsCommand command)
        {
            command.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();

            await Validate(command);

            Mapper.Map(command, command.Model);

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}