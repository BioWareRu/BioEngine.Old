namespace BioEngine.Data.Core
{
    public abstract class UpdateCommand<TEntity> : CreateCommand<bool>
    {
        public virtual TEntity Model { get; protected set; }
    }
}