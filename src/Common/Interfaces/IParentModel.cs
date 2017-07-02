using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Base;

namespace BioEngine.Common.Interfaces
{
    public interface IParentModel
    {
        object GetId();

        ParentType Type { get; }

        string Icon { get; }

        string ParentUrl { get; }

        string DisplayTitle { get; }
    }
}