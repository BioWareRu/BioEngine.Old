using System.Collections.Generic;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetCategoryFilesQuery : QueryBase<(IEnumerable<File> files, int count)>
    {
        public int PageSize { get; set; } = 20;

        public GetCategoryFilesQuery(FileCat cat, int page = 1)
        {
            Cat = cat;
            Page = page;
        }

        public FileCat Cat { get; }
        public int Page { get; }
    }
}