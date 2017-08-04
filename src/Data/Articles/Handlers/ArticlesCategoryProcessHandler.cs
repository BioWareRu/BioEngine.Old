using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Requests;
using BioEngine.Data.Core;
using MediatR;

namespace BioEngine.Data.Articles.Handlers
{
    public class ArticlesCategoryProcessHandler : CategoryProcessHandlerBase<ArticleCategoryProcessRequest, ArticleCat, Article>
    {
        public ArticlesCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<Article>> GetCatItems(ArticleCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryArticlesRequest(cat) {PageSize = count})).articles;
        }
    }
}