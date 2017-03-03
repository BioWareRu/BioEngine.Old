﻿using BioEngine.Common.Base;
using JsonApiDotNetCore.Models;

namespace BioEngine.Common.Interfaces
{
    public interface IParentModel 
    {
        int Id { get; set; }

        ParentType Type { get; }
        string Icon { get; }
        string ParentUrl { get; }
        string DisplayTitle { get; }
    }
}