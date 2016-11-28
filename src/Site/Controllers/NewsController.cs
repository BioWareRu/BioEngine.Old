using System;
using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Site.Base;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(BWContext context) : base(context)
        {
        }

        [Route("/{year}/{month}/{day}/{url}.html")]
        public ViewResult Show(int year, int month, int day, string url)
        {
            var dateStart = new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0)).ToUnixTimeSeconds();
            var dateEnd = new DateTimeOffset(new DateTime(year, month, day, 23, 59, 59)).ToUnixTimeSeconds();

            var news =
                Context.News.FirstOrDefault(
                    n => (n.Pub == 1) && (n.Date >= dateStart) && (n.Date <= dateEnd) && (n.Url == url));

            return news == null ? new ViewResult {StatusCode = 404} : View(news);
        }
    }
}