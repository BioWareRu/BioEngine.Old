using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewComponents
{
    public class CounterViewComponent : ViewComponent
    {
        private readonly BWContext dbContext;

        public CounterViewComponent(BWContext context)
        {
            dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var block = await dbContext.Blocks.FirstOrDefaultAsync((x => x.Index == "counter"));
            return View(block);
        }
    }
}
