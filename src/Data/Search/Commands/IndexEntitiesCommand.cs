using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Data.Core;

namespace BioEngine.Data.Search.Commands
{
    public class IndexEntitiesCommand<T> : CommandBase where T : IBaseModel
    {
        public IndexEntitiesCommand(IEnumerable<T> models)
        {
            Models = models;
        }

        public IEnumerable<T> Models { get; }
    }
}