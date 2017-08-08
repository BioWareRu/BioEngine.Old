using BioEngine.Common.Base;

namespace BioEngine.Data.Core
{
    public abstract class SingleModelQueryBase<T> : QueryBase<T> where T : IBaseModel
    {
    }
}