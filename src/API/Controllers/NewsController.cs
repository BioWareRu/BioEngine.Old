using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components.REST;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.API.Controllers
{
    public class NewsController : RestController<News, int>
    {
        public NewsController(BWContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<News> GetBaseQuery()
        {
            return DBContext.News.Include(x => x.Game).Include(x => x.Developer).Include(x => x.Topic)
                .Include(x => x.Author);
        }

        protected override Task<News> GetItem(int id)
        {
            return GetBaseQuery().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}