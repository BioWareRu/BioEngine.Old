using BioEngine.Common.Interfaces;

namespace BioEngine.Data.Core
{
    public interface ICategoryRequest<out T> where T : class, ICat
    {
        IParentModel Parent { get; }

        bool LoadChildren { get; }

        T ParentCat { get; }

        int? LoadLastItems { get; }

        string Url { get; }
    }
}