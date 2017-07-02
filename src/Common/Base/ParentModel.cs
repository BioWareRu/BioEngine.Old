using BioEngine.Common.Interfaces;
using JetBrains.Annotations;

namespace BioEngine.Common.Base
{
    public abstract class ParentModel<TPkType> : BaseModel<TPkType>, IParentModel
    {
        [UsedImplicitly]
        public virtual ParentType Type { get; }

        [UsedImplicitly]
        public virtual string Icon { get; }

        [UsedImplicitly]
        public virtual string ParentUrl { get; }

        [UsedImplicitly]
        public virtual string DisplayTitle { get; }
    }

    public enum ParentType
    {
        Game = 1,
        Developer = 2,
        Topic = 3
    }
}