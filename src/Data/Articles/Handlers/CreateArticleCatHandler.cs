using System.Threading.Tasks;
using BioEngine.Data.Articles.Commands;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using FluentValidation;

namespace BioEngine.Data.Articles.Handlers
{
    internal class CreateArticleCatHandler : RestCommandHandlerBase<CreateArticleCatCommand, int>
    {
        public CreateArticleCatHandler(HandlerContext<CreateArticleCatHandler> context, 
            IValidator<CreateArticleCatCommand>[] validators) 
            : base(context, validators)
        {
        }

        protected override async Task<int> ExecuteCommandAsync(CreateArticleCatCommand command)
        {
            var articleCat = Mapper.Map<CreateArticleCatCommand, Common.Models.ArticleCat>(command);
            DBContext.ArticleCats.Add(articleCat);
            await DBContext.SaveChangesAsync();
            
            DBContext.Entry(articleCat)
                .Reference(fc => fc.Game)
                .Load();
            DBContext.Entry(articleCat)
                .Reference(fc => fc.Developer)
                .Load();
            
            await Mediator.Publish(new IndexEntityCommand<Common.Models.ArticleCat>(articleCat));

            return articleCat.Id;
        }
    }
}