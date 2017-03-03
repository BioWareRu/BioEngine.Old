using System;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;

namespace BioEngine.Site.ViewModels
{
    public class ParentHeaderViewModel
    {
        public ParentHeaderViewModel(IParentModel parent, Func<IParentModel, Task<string>> getUrl,
            Func<IParentModel, Task<string>> getIconUrl)
        {
            Parent = parent;
            GetUrl = getUrl;
            GetIconUrl = getIconUrl;
        }

        public IParentModel Parent { get; }

        public readonly Func<IParentModel, Task<string>> GetUrl;
        public readonly Func<IParentModel, Task<string>> GetIconUrl;
    }
}