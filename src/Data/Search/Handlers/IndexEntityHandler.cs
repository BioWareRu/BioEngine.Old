using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using BioEngine.Search.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Data.Search.Handlers
{
    [UsedImplicitly]
    internal class IndexEntityHandler<T> : CommandHandlerBase<IndexEntityCommand<T>> where T : IBaseModel
    {
        private readonly ISearchProvider<T> _searchProvider;

        public IndexEntityHandler(HandlerContext<IndexEntityHandler<T>> context, ISearchProvider<T> searchProvider) : base(context)
        {
            _searchProvider = searchProvider;
        }

        protected override async Task ExecuteCommand(IndexEntityCommand<T> command)
        {
           await _searchProvider.AddUpdateEntity(command.Model);
        }
    }
}