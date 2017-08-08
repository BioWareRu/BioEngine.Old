using System.Collections.Generic;
using BioEngine.Common.Base;

namespace BioEngine.Common.Interfaces
{
    public interface ICat : IBaseModel
    {
        int Id { get; set; }
    }

    public interface ICat<TCat, TEntity> : ICat, IChildModel
    {
        List<TCat> Children { get; set; }
        IEnumerable<TEntity> Items { get; set; }
    }
}