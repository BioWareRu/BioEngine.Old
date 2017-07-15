using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;

namespace BioEngine.Site.ViewModels.News
{
    public class ParentNewsListViewModel : BaseViewModel
    {
        public ParentNewsListViewModel(BaseViewModelConfig config, IParentModel parent,
            IEnumerable<Common.Models.News> news, int totalNews,
            int currentPage) : base(config)
        {
            Parent = parent;
            News = news;
            TotalNews = totalNews;
            CurrentPage = currentPage;
        }

        public IParentModel Parent { get; }
        public IEnumerable<Common.Models.News> News { get; }

        public int TotalNews { get; }
        public int CurrentPage { get; }

        public override Task<string> Title()
        {
            return Task.FromResult($"{Parent.DisplayTitle} - Новости");
        }

        public override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Новости раздела ${Parent.DisplayTitle}");
        }
    }
}