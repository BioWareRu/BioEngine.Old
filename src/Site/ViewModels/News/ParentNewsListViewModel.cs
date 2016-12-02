using System.Collections.Generic;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels.News
{
    public class ParentNewsListViewModel : BaseViewModel
    {
        public ParentNewsListViewModel(IEnumerable<Settings> settings, ParentModel parent,
            IEnumerable<Common.Models.News> news, int totalNews,
            int currentPage) : base(settings)
        {
            Parent = parent;
            News = news;
            TotalNews = totalNews;
            CurrentPage = currentPage;
            Title = parent.DisplayTitle + " - Новости";
        }

        public ParentModel Parent { get; set; }
        public IEnumerable<Common.Models.News> News { get; }

        public int TotalNews { get; }
        public int CurrentPage { get; }
    }
}