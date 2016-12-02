using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ParentArticlesViewModel : BaseViewModel
    {
        public List<ChildCat> Cats;
        public ParentModel Parent;
        private UrlManager UrlManager;

        public ParentArticlesViewModel(IEnumerable<Settings> settings, ParentModel parent, List<ChildCat> cats,
            UrlManager urlManager) : base(settings)
        {
            Parent = parent;
            Cats = cats;
            UrlManager = urlManager;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
        }

        public string ParentArticlesUrl => UrlManager.Articles.ParentArticlesUrl((dynamic) Parent);
        public string ParentIconUrl => UrlManager.ParentIconUrl((dynamic) Parent);
    }
}