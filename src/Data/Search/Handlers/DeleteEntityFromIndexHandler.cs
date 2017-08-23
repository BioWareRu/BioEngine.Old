using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Commands;
using BioEngine.Search.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Data.Search.Handlers
{
    [UsedImplicitly]
    internal class DeleteEntityFromIndexHandler<T> : CommandHandlerBase<DeleteEntityFromIndexCommand<T>> where T : IBaseModel
    {
        private readonly ISearchProvider<T> _searchProvider;

        public DeleteEntityFromIndexHandler(HandlerContext<DeleteEntityFromIndexHandler<T>> context, ISearchProvider<T> searchProvider) : base(context)
        {
            _searchProvider = searchProvider;
        }

        protected override async Task ExecuteCommandAsync(DeleteEntityFromIndexCommand<T> command)
        {
           await _searchProvider.DeleteEntityAsync(command.Model);
        }
    }
}