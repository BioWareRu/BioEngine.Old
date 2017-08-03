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
        private Func<int, Uri> UrlGenerator { get; }

        public PagerModel(int currentPage, int totalCount, Func<int, Uri> urlGenerator, int itemsPerPage = 10)
        {
            CurrentPage = currentPage;
            PageCount = (int) Math.Ceiling((double) totalCount/itemsPerPage);
            UrlGenerator = urlGenerator;
        }

        public Uri FirstLink()
        {
            return PageLink(1);
        }

        public Uri LastLink()
        {
            return PageLink(PageCount);
        }

        public Uri PrevLink()
        {
            return CurrentPage > 1 ? PageLink(CurrentPage - 1) : null;
        }

        public Uri NextLink()
        {
            return CurrentPage < PageCount ? PageLink(CurrentPage + 1) : null;
        }

        public Uri PageLink(int page)
        {
            return UrlGenerator(page);
        }
    }
}