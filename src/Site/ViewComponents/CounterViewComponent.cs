using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.ViewComponents
{
    public class CounterViewComponent : ViewComponent
    {
        private readonly BWContext _dbContext;

        public CounterViewComponent(BWContext context)
        {
            _dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Index == "counter");
            return View(block);
        }
    }
}