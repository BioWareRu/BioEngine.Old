using BioEngine.Common.Base;

namespace BioEngine.Common.Interfaces
{
    public interface IParentModel
    {
        ParentType Type { get; }

        string Icon { get; }

        string ParentUrl { get; }

        string DisplayTitle { get; }
        object GetId();
    }
}