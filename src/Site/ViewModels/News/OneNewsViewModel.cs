using System;
using System.Threading.Tasks;
using BioEngine.Site.Helpers;

namespace BioEngine.Site.ViewModels.News
{
    public class OneNewsViewModel : BaseViewModel
    {
        public OneNewsViewModel(BaseViewModelConfig config, Common.Models.News news) : base(config)
        {
            News = news;
        }

        public Common.Models.News News { get; private set; }

        public override Task<string> Title()
        {
            return Task.FromResult(News.Title);
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