using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class UnPinNewsHandler : RestCommandHandlerBase<UnPinNewsCommand, bool>
    {
        public UnPinNewsHandler(HandlerContext<UnPinNewsHandler> context, IValidator<UnPinNewsCommand>[] validators) :
            base(context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(UnPinNewsCommand command)
        {
            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.Model.Sticky = 0;

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            return true;
        }
    }
}