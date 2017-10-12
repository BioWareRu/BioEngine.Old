using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Files
{
    public class ParentFilesViewModel : BaseViewModel
    {
        public IEnumerable<FileCat> Cats;
        public IParentModel Parent;

        public ParentFilesViewModel(BaseViewModelConfig config, IParentModel parent,
            IEnumerable<FileCat> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
        }

        public override string Title()
        {
            return $"{Parent.DisplayTitle} - Файлы";
        }

        protected override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Файлы в разделе «{Parent.DisplayTitle}»");
        }
    }
}