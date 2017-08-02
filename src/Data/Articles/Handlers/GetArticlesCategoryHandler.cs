using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    public class GetArticlesCategoryHandler : GetArticlesCategoryHandlerBase<GetArticlesCategoryRequest, ArticleCat>
    {
        public GetArticlesCategoryHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        public override async Task<ArticleCat> Handle(GetArticlesCategoryRequest message)
        {
            var catQuery = DBContext.ArticleCats.AsQueryable();

            if (!string.IsNullOrEmpty(message.Url))
            {
                catQuery = catQuery.Where(x => x.Url == message.Url);
            }

            if (message.Parent != null)
            {
                catQuery = ApplyParentCondition(catQuery, message.Parent);
            }

            if (message.ParentCat != null)
            {
                catQuery = catQuery.Where(x => x.Pid == message.ParentCat.Id);
            }
            else
            {
                catQuery = catQuery.Include(x => x.ParentCat);
            }

            var cat = await catQuery.FirstOrDefaultAsync();
            if (cat != null)
            {
                ProcessCat(cat, message);
            }
            return cat;
        }
    }
}