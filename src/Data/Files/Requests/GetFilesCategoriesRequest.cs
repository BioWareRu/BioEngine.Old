using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetFilesCategoriesRequest : RequestBase<IEnumerable<FileCat>>, ICategoryRequest<FileCat>
    {
        public GetFilesCategoriesRequest(IParentModel parent = null, FileCat parentCat = null,
            string url = null, bool loadChildren = false, int? loadLastItems = null, bool onlyRoot = false)
        {
            Url = url;
            Parent = parent;
            ParentCat = parentCat;
            LoadChildren = loadChildren;
            LoadLastItems = loadLastItems;
            OnlyRoot = onlyRoot;
        }

        public bool OnlyRoot { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public FileCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}