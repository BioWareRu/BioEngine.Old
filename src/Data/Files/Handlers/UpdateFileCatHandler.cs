using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using BioEngine.Data.Search.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class UpdateFileCatHandler : RestCommandHandlerBase<UpdateFileCatCommand, bool>
    {
        public UpdateFileCatHandler(HandlerContext<UpdateFileCatHandler> context,
            IValidator<UpdateFileCatCommand>[] validators)
            :  base(context, validators)
        {
        }
        
        protected override async Task<bool> ExecuteCommandAsync(UpdateFileCatCommand command)
        {
            Mapper.Map(command, command.Model);
            
            await Mediator.Publish(new IndexEntityCommand<Common.Models.FileCat>(command.Model));

            DBContext.Update(command.Model);
            await DBContext.SaveChangesAsync();
            return true;
        }
    }
}