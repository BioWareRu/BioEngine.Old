using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace BioEngine.API.Components.REST.Models
{
    public class SaveModelReponse<T> : RestResult
    {
        [UsedImplicitly]
        public T Model { get; }

        public SaveModelReponse(int code, T model) : base(code)
        {
            Model = model;
        }
    }
}
