using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel : BaseModel
    {
        public abstract string NewsUrl { get; }

        public abstract string Icon { get; }

        public abstract string DisplayTitle { get; }
    }
}