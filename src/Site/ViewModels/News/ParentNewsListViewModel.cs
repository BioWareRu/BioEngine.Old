using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;

namespace BioEngine.Site.ViewModels.News
{
    public class ParentNewsListViewModel : BaseViewModel
    {
        public ParentNewsListViewModel(BaseViewModelConfig config, ParentModel parent,
            IEnumerable<Common.Models.News> news, int totalNews,
            int currentPage) : base(config)
        {
            Parent = parent;
            News = news;
            TotalNews = totalNews;
            CurrentPage = currentPage;
        }

        public ParentModel Parent { get; }
        public IEnumerable<Common.Models.News> News { get; }

        public int TotalNews { get; }
        public int CurrentPage { get; }

        public override Task<string> Title()
        {
            return Task.FromResult($"{Parent.DisplayTitle}- Новости");
        }
    }
}