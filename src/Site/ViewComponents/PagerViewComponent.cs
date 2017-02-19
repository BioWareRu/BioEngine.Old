using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PagerModel pagerModel)
        {
            return await Task.Run(() => View(pagerModel));
        }
    }

    public struct PagerModel
    {
        public int CurrentPage { get; }
        public int PageCount { get; }
        private Func<int, Task<string>> UrlGenerator { get; }

        public PagerModel(int currentPage, int totalCount, Func<int, Task<string>> urlGenerator, int itemsPerPage = 10)
        {
            CurrentPage = currentPage;
            PageCount = (int) Math.Ceiling((double) totalCount/itemsPerPage);
            UrlGenerator = urlGenerator;
        }

        public async Task<string> FirstLink()
        {
            return await PageLink(1);
        }

        public async Task<string> LastLink()
        {
            return await PageLink(PageCount);
        }

        public async Task<string> PrevLink()
        {
            return CurrentPage > 1 ? await PageLink(CurrentPage - 1) : null;
        }

        public async Task<string> NextLink()
        {
            return CurrentPage < PageCount ? await PageLink(CurrentPage + 1) : null;
        }

        public async Task<string> PageLink(int page)
        {
            return await UrlGenerator(page);
        }
    }
}