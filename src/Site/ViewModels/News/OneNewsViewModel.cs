using System;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.News
{
    public class OneNewsViewModel : BaseViewModel
    {
        public OneNewsViewModel(BaseViewModelConfig config, Common.Models.News news) : base(config)
        {
            News = news;
        }

        public Common.Models.News News { get; }

        public override string Title()
        {
            return News.Title;
        }

        protected override async Task<string> GetDescription()
        {
            return await Task.FromResult(GetDescriptionFromHtml(News.ShortText));
        }

        public override Uri GetImageUrl()
        {
            return GetImageFromHtml(News.ShortText) ?? ImageUrl;
        }
    }
}