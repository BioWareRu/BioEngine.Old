using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel : BaseModel
    {
        public abstract int Id { get; set; }

        [NotMapped]
        public abstract ParentType Type { get; set; }

        public abstract string NewsUrl { get; }

        public abstract string Icon { get; }
        public abstract string ParentUrl { get; }

        public abstract string DisplayTitle { get; }
    }

    public enum ParentType
    {
        Game = 1,
        Developer = 2,
        Topic = 3
    }
}