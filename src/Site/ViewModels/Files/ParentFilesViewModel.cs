using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Files
{
    public class ParentFilesViewModel : BaseViewModel
    {
        public List<CatsTree<FileCat, File>> Cats;
        public ParentModel Parent;

        public ParentFilesViewModel(BaseViewModelConfig config, ParentModel parent,
            List<CatsTree<FileCat, File>> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
        }

        public override Task<string> Title()
        {
            return Task.FromResult($"{Parent.DisplayTitle} - Файлы");
        }
    }
}