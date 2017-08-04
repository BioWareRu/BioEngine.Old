using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetFilesQuery : QueryBase<(IEnumerable<Common.Models.File> files, int count)>
    {
        public int? Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public GetFilesQuery(int? page = null, IParentModel parent = null)
        {
            Page = page;
            Parent = parent;
        }
    }
}