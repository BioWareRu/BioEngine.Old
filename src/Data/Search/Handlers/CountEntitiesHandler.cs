using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Queries;
using BioEngine.Search.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Data.Search.Handlers
{
    [UsedImplicitly]
    internal class CountEntitiesHandler<T> : QueryHandlerBase<CountEntitiesQuery<T>, long>
        where T : IBaseModel
    {
        private readonly ISearchProvider<T> _searchProvider;

        public CountEntitiesHandler(HandlerContext<CountEntitiesHandler<T>> context, ISearchProvider<T> searchProvider) : base(context)
        {
            _searchProvider = searchProvider;
        }

        protected override async Task<long> RunQuery(CountEntitiesQuery<T> message)
        {
            return await _searchProvider.Count(message.Query);
        }
    }
}