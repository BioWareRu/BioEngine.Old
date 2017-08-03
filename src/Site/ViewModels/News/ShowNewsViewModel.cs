using System;
using BioEngine.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewModels.News
{
    public class ShowNewsViewModel
    {
        private readonly IUrlHelper _urlHelper;

        public ShowNewsViewModel(Common.Models.News news, bool showFull, IUrlHelper urlHelper, Uri imageUrl)
        {
            _urlHelper = urlHelper;
            News = news;
            ShowFull = showFull;
            Image = imageUrl;
        }

        public Common.Models.News News { get; }
        public bool ShowFull { get; }

        public DateTimeOffset Date => DateTimeOffset.FromUnixTimeSeconds(News.Date);

        public Uri CommentsUrl => _urlHelper.News().CommentsUrl(News);

        public Uri NewsUrl => _urlHelper.News().PublicUrl(News, true);
        public Uri Image { get; }
        public string Description => BaseViewModel.GetDescriptionFromHtml(News.ShortText);
    }
}