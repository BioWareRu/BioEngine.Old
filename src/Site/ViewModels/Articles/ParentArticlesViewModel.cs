using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.Articles
{
    public class ParentArticlesViewModel : BaseViewModel
    {
        public readonly IEnumerable<ArticleCat> Cats;
        public readonly IParentModel Parent;

        public ParentArticlesViewModel(BaseViewModelConfig config, IParentModel parent,
            IEnumerable<ArticleCat> cats) : base(config)
        {
            Parent = parent;
            Cats = cats;
        }

        public override string Title()
        {
            return $"{Parent.DisplayTitle} - Статьи";
        }

        protected override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Статьи в разделе «{Parent.DisplayTitle}»");
        }
    }
}