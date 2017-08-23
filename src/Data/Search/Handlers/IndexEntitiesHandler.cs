using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using BioEngine.Search.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Data.Search.Handlers
{
    [UsedImplicitly]
    internal class IndexEntitiesHandler<T> : CommandHandlerBase<IndexEntitiesCommand<T>>
        where T : IBaseModel
    {
        private readonly ISearchProvider<T> _searchProvider;

        public IndexEntitiesHandler(HandlerContext<IndexEntitiesHandler<T>> context, ISearchProvider<T> searchProvider) : base(context)
        {
            _searchProvider = searchProvider;
        }

        protected override async Task ExecuteCommandAsync(IndexEntitiesCommand<T> command)
        {
            await _searchProvider.AddOrUpdateEntitiesAsync(command.Models);
        }
    }
}