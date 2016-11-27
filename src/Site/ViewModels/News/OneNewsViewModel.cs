using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Site.Helpers;
using BioEngine.Common.Models;

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