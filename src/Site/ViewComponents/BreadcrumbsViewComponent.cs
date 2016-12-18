using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.ViewComponents
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<BreadCrumbsItem> items)
        {
            return await Task.Run(() => View(items));
        }
    }
}