using System;
using BioEngine.Common.Interfaces;

namespace BioEngine.Site.ViewModels
{
    public class ParentHeaderViewModel
    {
        public ParentHeaderViewModel(IParentModel parent, Func<IParentModel, Uri> getUrl,
            Func<IParentModel, Uri> getIconUrl)
        {
            Parent = parent;
            GetUrl = getUrl;
            GetIconUrl = getIconUrl;
        }

        public IParentModel Parent { get; }

        public readonly Func<IParentModel, Uri> GetUrl;
        public readonly Func<IParentModel, Uri> GetIconUrl;
    }
}