using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetFilesCategoryRequest : RequestBase<FileCat>, ICategoryRequest<FileCat>
    {
        public GetFilesCategoryRequest(IParentModel parent = null, FileCat parentCat = null,
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