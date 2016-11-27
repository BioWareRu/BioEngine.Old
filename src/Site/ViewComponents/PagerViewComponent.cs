using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PagerModel pagerModel)
        {
            return await Task.Run(() =>
            {
                return View(pagerModel);
            });
        }
    }

    public struct PagerModel
    {
        public int CurrentPage { get; }
        public int PageCount { get; }
        private Func<int, string> UrlGenerator { get; }

        public PagerModel(int currentPage, int totalCount, Func<int, string> urlGenerator, int itemsPerPage = 10)
        {
            CurrentPage = currentPage;
            PageCount = (int)Math.Ceiling((double)totalCount / itemsPerPage);
            UrlGenerator = urlGenerator;
        }

        public string FirstLink()
        {
            return PageLink(1);
        }

        public string LastLink()
        {
            return PageLink(PageCount);
        }

        public string PrevLink()
        {
            return CurrentPage > 1 ? PageLink(CurrentPage - 1) : null;
        }

        public string NextLink()
        {
            return CurrentPage < PageCount ? PageLink(CurrentPage + 1) : null;
        }

        public string PageLink(int page)
        {
            return UrlGenerator(page);
        }
    }
}
