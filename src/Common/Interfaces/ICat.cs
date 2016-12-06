using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Common.Interfaces
{
    public interface ICat<TCat>
    {
        int Id { get; set; }
        List<TCat> Children { get; set; }
    }
}