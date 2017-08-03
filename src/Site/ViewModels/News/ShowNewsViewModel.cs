using System;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels.News
{
    public class ShowNewsViewModel
    {
        public ShowNewsViewModel(Common.Models.News news, bool showFull, UrlManager urlManager)
        {
            News = news;
            ShowFull = showFull;
            UrlManager = urlManager;
        }

        public Common.Models.News News { get; }
        public bool ShowFull { get; }
        private UrlManager UrlManager { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(News.Date);

        public string CommentsUrl => UrlManager.News.CommentsUrl(News);

        public Uri NewsUrl => new Uri(UrlManager.News.PublicUrl(News, true));

        public Uri Image => BaseViewModel.GetImageFromHtml(News.ShortText);
        public string Description => BaseViewModel.GetDescriptionFromHtml(News.ShortText);
    }
}