using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetFileCategoryByIdQuery : SingleModelQueryBase<FileCat>, ICategoryQuery<FileCat>
    {
        public GetFileCategoryByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public IParentModel Parent { get; }
        public bool LoadChildren { get; }
        public FileCat ParentCat { get; }
        public int? LoadLastItems { get; }
        public string Url { get; }
    }
}