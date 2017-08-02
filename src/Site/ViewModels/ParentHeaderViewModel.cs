using System;
using BioEngine.Common.Interfaces;

namespace BioEngine.Site.ViewModels
{
    public class ParentHeaderViewModel
    {
        public ParentHeaderViewModel(IParentModel parent, Func<IParentModel, string> getUrl,
            Func<IParentModel, string> getIconUrl)
        {
            Parent = parent;
            GetUrl = getUrl;
            GetIconUrl = getIconUrl;
        }

        public IParentModel Parent { get; }

        public readonly Func<IParentModel, string> GetUrl;
        public readonly Func<IParentModel, string> GetIconUrl;
    }
}