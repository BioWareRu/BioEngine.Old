using System;
using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class PinNewsHandler : RestCommandHandlerBase<PinNewsCommand, bool>
    {
        public PinNewsHandler(HandlerContext<PinNewsHandler> context, IValidator<PinNewsCommand>[] validators) : base(
            context, validators)
        {
        }

        protected override async Task<bool> ExecuteCommandAsync(PinNewsCommand command)
        {
            //find old pinned news
            var alreadyPinnedNews = await DBContext.News.FirstOrDefaultAsync(x => x.Sticky == 1);
            if (alreadyPinnedNews != null)
            {
                //unpin old news
                await Mediator.Send(new UnPinNewsCommand(alreadyPinnedNews));
            }

            command.Model.LastChangeDate = DateTimeOffset.Now.ToUnixTimeSeconds();
            command.Model.Sticky = 1;

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();

            return true;
        }
    }
}