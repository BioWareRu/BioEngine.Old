using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetFilesRequest : RequestBase<(IEnumerable<Common.Models.File> articles, int count)>
    {
        public int Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public GetFilesRequest(int page = 1, IParentModel parent = null)
        {
            Page = page;
            Parent = parent;
        }
    }
}