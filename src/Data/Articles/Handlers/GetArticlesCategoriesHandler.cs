using System.Collections.Generic;
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
    class GetArticlesCategoriesHandler : GetArticlesCategoryHandlerBase<GetArticlesCategoriesRequest,
        IEnumerable<ArticleCat>>
    {
        public GetArticlesCategoriesHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        public override async Task<IEnumerable<ArticleCat>> Handle(GetArticlesCategoriesRequest message)
        {
            var query = DBContext.ArticleCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.Pid == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.Pid == null);
            }


            var cats = await query.ToListAsync();
            foreach (var cat in cats)
            {
                ProcessCat(cat, message);
            }

            return cats;
        }
    }
}