using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BWContext = BioEngine.Common.DB.BWContext;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BioEngine.Site.Controllers
{
    public class IndexController : BaseController
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return NewsList(1);
        }

        [HttpGet("/page/{page}.html")]
        public IActionResult Index(int page)
        {
            return NewsList(page);
        }

        private IActionResult NewsList(int page)
        {
            var news =
                Context.News.OrderByDescending(x => x.Date)
                    .Include(x => x.Author)
                    .Include(x => x.Game)
                    .Include(x => x.Developer)
                    .Include(x => x.Topic)
                    .Skip((page - 1) * 20)
                    .Take(20)
                    .ToList();
            var totalNews = Context.News.Count();

            return View(new NewsListViewModel(Settings, news, totalNews, page) { Title = "Новости" });
        }

        public IndexController(BWContext context) : base(context)
        {
        }
    }
}