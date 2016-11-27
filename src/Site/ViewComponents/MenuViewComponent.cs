using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BWContext = BioEngine.Common.DB.BWContext;

namespace BioEngine.Site.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly BWContext dbContext;

        public MenuViewComponent(BWContext context)
        {
            dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            var menu = await GetItemsAsync(key);
            return View(menu.GetMenu());
        }

        private Task<Menu> GetItemsAsync(string key)
        {
            return dbContext.Menus.Where(x => x.Key == key).FirstOrDefaultAsync();
        }
    }
}