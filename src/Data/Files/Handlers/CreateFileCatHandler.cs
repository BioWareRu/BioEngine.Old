using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class CreateFileCatHandler : RestCommandHandlerBase<CreateFileCatCommand, int>
    {
        public CreateFileCatHandler(HandlerContext<CreateFileCatHandler> context, 
            IValidator<CreateFileCatCommand>[] validators) : base(context, validators)
        {
        }

        protected override async Task<int> ExecuteCommandAsync(CreateFileCatCommand command)
        {
            var fileCat = Mapper.Map<CreateFileCatCommand, Common.Models.FileCat>(command);
            DBContext.FileCats.Add(fileCat);
            await DBContext.SaveChangesAsync();

            return fileCat.Id;
        }
    }
}