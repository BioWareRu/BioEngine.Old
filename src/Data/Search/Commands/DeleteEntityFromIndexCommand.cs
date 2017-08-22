using BioEngine.Common.Base;
using BioEngine.Data.Core;

namespace BioEngine.Data.Search.Commands
{
    public class DeleteEntityFromIndexCommand<T> : CommandBase where T : IBaseModel
    {
        public DeleteEntityFromIndexCommand(T model)
        {
            Model = model;
        }

        public T Model { get; }
    }
}