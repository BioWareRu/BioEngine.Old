using System.Collections.Generic;
using BioEngine.Common.Models;
using BioEngine.Site.Helpers;

namespace BioEngine.Site.ViewModels.News
{
    public class OneNewsViewModel : BaseViewModel
    {
        public OneNewsViewModel(Common.Models.News news, IEnumerable<Settings> settings) : base(settings)
        {
            News = news;
            Title = news.Title;
            ImageUrl = ContentHelper.GetImageUrl(news.ShortText);
            Description = ContentHelper.GetDescription(news.ShortText);
        }

        public Common.Models.News News { get; private set; }
    }
}