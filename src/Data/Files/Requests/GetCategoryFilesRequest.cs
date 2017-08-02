using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetCategoryFilesRequest : RequestBase<(IEnumerable<File> files, int count)>
    {
        public int PageSize { get; set; } = 20;

        public GetCategoryFilesRequest(FileCat cat, int page = 1)
        {
            Cat = cat;
            Page = page;
        }

        public FileCat Cat { get; }
        public int Page { get; }
    }
}