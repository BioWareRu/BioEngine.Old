using JetBrains.Annotations;

namespace BioEngine.API.Components.REST.Models
{
    public class SaveModelReponse<T> : RestResult
    {
        public SaveModelReponse(int code, T model) : base(code)
        {
            Model = model;
        }

        [UsedImplicitly]
        public T Model { get; }
    }
}