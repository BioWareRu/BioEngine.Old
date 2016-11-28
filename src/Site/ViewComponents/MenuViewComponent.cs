using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly BWContext _dbContext;

        public MenuViewComponent(BWContext context)
        {
            _dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            var menu = await GetItemsAsync(key);
            return View(menu.GetMenu());
        }

        private Task<Menu> GetItemsAsync(string key)
        {
            return _dbContext.Menus.Where(x => x.Key == key).FirstOrDefaultAsync();
        }
    }
}