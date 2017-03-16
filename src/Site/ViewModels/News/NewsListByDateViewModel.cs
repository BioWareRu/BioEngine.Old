using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.News
{
    public class NewsListByDateViewModel : NewsListViewModel
    {
        public int Year { get; }
        public int? Month { get; }
        public int? Day { get; }

        public NewsListByDateViewModel(BaseViewModelConfig config, IEnumerable<Common.Models.News> news, int totalNews,
            int currentPage, int year, int? month, int? day) : base(config, news, totalNews, currentPage)
        {
            Year = year;
            Month = month;
            Day = day;
        }
    }
}