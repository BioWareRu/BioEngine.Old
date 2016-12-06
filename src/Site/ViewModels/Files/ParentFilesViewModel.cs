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
        private UrlManager UrlManager;

        public ParentFilesViewModel(IEnumerable<Settings> settings, ParentModel parent,
            List<CatsTree<FileCat, File>> cats,
            UrlManager urlManager) : base(settings)
        {
            Parent = parent;
            Cats = cats;
            UrlManager = urlManager;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
            Title = $"{Parent.DisplayTitle} - Файлы";
        }

        public string ParentArticlesUrl => UrlManager.Files.ParentFilesUrl((dynamic) Parent);
        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) Parent);
    }
}