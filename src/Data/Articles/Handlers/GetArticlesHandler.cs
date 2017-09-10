using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Core;
using BioEngine.Routing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Articles.Handlers
{
    [UsedImplicitly]
    internal class GetArticlesHandler : ModelListQueryHandlerBase<GetArticlesQuery, Common.Models.Article>
    {
        private readonly BioUrlManager _urlManager;

        public GetArticlesHandler(HandlerContext<GetArticlesHandler> context, BioUrlManager urlManager) : base(context)
        {
            _urlManager = urlManager;
        }

        protected override async Task<(IEnumerable<Common.Models.Article>, int)> RunQueryAsync(GetArticlesQuery message)
        {
            var query = DBContext.Articles.AsQueryable();
            if (!message.WithUnPublishedArticles)
                query = query.Where(x => x.Pub == 1);
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }
            
            query = query
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic)
                .Include(x => x.Cat);

            var data = await GetDataAsync(query, message);
            foreach (var article in data.models)
            {
                article.Cat =
                    await Mediator.Send(new ArticleCategoryProcessQuery(article.Cat,
                        new GetArticlesCategoryQuery {Parent = message.Parent}));
                article.PublicUrl = _urlManager.Articles.PublicUrl(article, true);
            }

            return data;
        }
    }
}