using System;
using System.Collections.Generic;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.News
{
    public class NewsListViewModel : BaseViewModel
    {
        public NewsListViewModel(IEnumerable<Settings> settings, IEnumerable<Common.Models.News> news, int totalNews, int currentPage) : base(settings)
        {
            News = news;
            TotalNews = totalNews;
            CurrentPage = currentPage;
        }

        public IEnumerable<Common.Models.News> News { get; }

        public int TotalNews { get; }
        public int CurrentPage { get; }
    }
}