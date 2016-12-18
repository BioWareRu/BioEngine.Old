using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;

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
            Title = $"{Parent.DisplayTitle} - Файлы";
        }

        public string ParentArticlesUrl => UrlManager.Files.ParentFilesUrl((dynamic) Parent);
        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) Parent);
    }
}