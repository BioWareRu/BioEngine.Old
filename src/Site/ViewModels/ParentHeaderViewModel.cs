using System;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Site.ViewModels
{
    public class ParentHeaderViewModel
    {
        public ParentHeaderViewModel(ParentModel parent, Func<ParentModel, Task<string>> getUrl,
            Func<ParentModel, Task<string>> getIconUrl)
        {
            Parent = parent;
            GetUrl = getUrl;
            GetIconUrl = getIconUrl;
        }

        public ParentModel Parent { get; }

        public readonly Func<ParentModel, Task<string>> GetUrl;
        public readonly Func<ParentModel, Task<string>> GetIconUrl;
    }
}