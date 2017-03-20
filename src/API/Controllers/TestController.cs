using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BioEngine.API.Controllers
{
    public class TestController : Controller
    {
        private readonly BWContext _dbContext;

        public TestController(BWContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResult> News([FromServices] ILogger<TestController> logger)
        {
            var stopwatch = Stopwatch.StartNew();
            var news = await _dbContext.News.Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Topic)
                .Take(20)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            stopwatch.Stop();
            logger.LogWarning($"Test request: {stopwatch.ElapsedMilliseconds}");
            return Json(news);
        }
    }
}
