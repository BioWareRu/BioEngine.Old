using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetFilesCategoryQuery : QueryBase<FileCat>, ICategoryQuery<FileCat>
    {
        public GetFilesCategoryQuery(IParentModel parent = null, FileCat parentCat = null,
            bool loadChildren = false,
            int? loadLastItems = null, string url = null)
        {
            Parent = parent;
            LoadChildren = loadChildren;
            ParentCat = parentCat;
            LoadLastItems = loadLastItems;
            Url = url;
        }

        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public FileCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}