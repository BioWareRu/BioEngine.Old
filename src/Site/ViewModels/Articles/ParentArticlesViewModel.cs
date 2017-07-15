using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ParentArticlesViewModel : BaseViewModel
    {
        public readonly List<CatsTree<ArticleCat, Article>> Cats;
        public readonly IParentModel Parent;

        public ParentArticlesViewModel(BaseViewModelConfig config, IParentModel parent,
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

        public override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Статьи в разделе \"{Parent.DisplayTitle}\"");
        }
    }
}