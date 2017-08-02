using System.Collections.Generic;

namespace BioEngine.Common.Interfaces
{
    public interface ICat<TCat, TEntity> : IChildModel
    {
        int Id { get; set; }
        List<TCat> Children { get; set; }
        IEnumerable<TEntity> Items { get; set; }
    }
}