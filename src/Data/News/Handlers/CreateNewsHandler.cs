using System.Threading.Tasks;
using BioEngine.Data.Core;
using BioEngine.Data.News.Commands;
using FluentValidation;
using JetBrains.Annotations;

namespace BioEngine.Data.News.Handlers
{
    [UsedImplicitly]
    internal class CreateNewsHandler : RestCommandHandlerBase<CreateNewsCommand, int>
    {
        public CreateNewsHandler(HandlerContext<CreateNewsHandler> context, IValidator<CreateNewsCommand>[] validators)
            : base(context, validators)
        {
        }

        protected override async Task<int> ExecuteCommandAsync(CreateNewsCommand createCommand)
        {
            var news = Mapper.Map<CreateNewsCommand, Common.Models.News>(createCommand);
            DBContext.News.Add(news);
            await DBContext.SaveChangesAsync();

            return news.Id;
        }
    }
}