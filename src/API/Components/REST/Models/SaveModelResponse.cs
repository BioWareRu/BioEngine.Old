using JetBrains.Annotations;

namespace BioEngine.API.Components.REST.Models
{
    public class SaveModelResponse<T> : RestResult
    {
        public SaveModelResponse(int code, T model) : base(code)
        {
            Model = model;
        }

        [UsedImplicitly]
        public T Model { get; }
    }
}