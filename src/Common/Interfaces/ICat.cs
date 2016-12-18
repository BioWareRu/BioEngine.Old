using System.Collections.Generic;

namespace BioEngine.Common.Interfaces
{
    public interface ICat<TCat> : IChildModel
    {
        int Id { get; set; }
        List<TCat> Children { get; set; }
    }
}