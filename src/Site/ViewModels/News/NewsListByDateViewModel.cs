using System;
using System.Collections.Generic;
using System.Text;
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

        public override Task<string> Title()
        {
            var monthStart = Month ?? 1;
            var dayStart = Day ?? 1;
            var date = new DateTime(Year, monthStart, dayStart);
            var dateStringBuilder = new StringBuilder();
            if (Day != null)
            {
                dateStringBuilder.Append(
                    $"{Day} {System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames[monthStart - 1]} {Year} года");
            }
            else
            {
                dateStringBuilder.Append(Month != null ? date.ToString("MMMM yyyy года") : $"{Year} год");
            }

            return Task.FromResult(SiteTitle + $" - Новости за {dateStringBuilder}");
        }
    }
}