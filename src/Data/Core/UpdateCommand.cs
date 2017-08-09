namespace BioEngine.Data.Core
{
    public abstract class UpdateCommand<TEntity> : CreateCommand<bool>
    {
        public abstract TEntity Model { get; protected set; }

        public void SetModel(TEntity model)
        {
            Model = model;
        }
    }
}