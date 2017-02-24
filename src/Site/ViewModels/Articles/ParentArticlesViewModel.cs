using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ParentArticlesViewModel : BaseViewModel
    {
        public readonly List<CatsTree<ArticleCat, Article>> Cats;
        public readonly ParentModel Parent;

        public ParentArticlesViewModel(BaseViewModelConfig config, ParentModel parent,
            List<CatsTree<ArticleCat, Article>> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
            BreadCrumbs.Add(new BreadCrumbsItem(UrlManager.ParentUrl(Parent), Parent.DisplayTitle));
        }

        public override Task<string> Title()
        {
            return Task.FromResult($"{Parent.DisplayTitle} - Статьи");
        }
    }
}