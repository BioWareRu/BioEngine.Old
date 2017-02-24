using System.Threading.Tasks;
using BioEngine.Site.Helpers;

namespace BioEngine.Site.ViewModels.News
{
    public class OneNewsViewModel : BaseViewModel
    {
        public OneNewsViewModel(BaseViewModelConfig config, Common.Models.News news) : base(config)
        {
            News = news;
            ImageUrl = ContentHelper.GetImageUrl(news.ShortText);
            Description = ContentHelper.GetDescription(news.ShortText);
        }

        public Common.Models.News News { get; private set; }
        public override Task<string> Title()
        {
            return Task.FromResult(News.Title);
        }
    }
}