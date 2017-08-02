using System.Collections.Generic;

namespace BioEngine.Common.Interfaces
{
    public interface ICat
    {
        int Id { get; set; }
    }

    public interface ICat<TCat, TEntity> : ICat, IChildModel
    {
        List<TCat> Children { get; set; }
        IEnumerable<TEntity> Items { get; set; }
    }
}