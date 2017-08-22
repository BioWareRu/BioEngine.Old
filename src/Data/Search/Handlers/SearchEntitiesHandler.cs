using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Queries;
using BioEngine.Search.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Data.Search.Handlers
{
    [UsedImplicitly]
    internal class SearchEntitiesHandler<T> : QueryHandlerBase<SearchEntitiesQuery<T>, IEnumerable<T>>
        where T : IBaseModel
    {
        private readonly ISearchProvider<T> _searchProvider;

        public SearchEntitiesHandler(HandlerContext<SearchEntitiesHandler<T>> context,
            ISearchProvider<T> searchProvider) : base(context)
        {
            _searchProvider = searchProvider;
        }

        protected override async Task<IEnumerable<T>> RunQuery(SearchEntitiesQuery<T> message)
        {
            return await _searchProvider.Search(message.Query, message.Limit);
        }
    }
}