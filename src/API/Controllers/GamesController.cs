using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components.REST;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.API.Controllers
{
    public class GamesController : RestController<Game, int>
    {
        public GamesController(BWContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Game> GetBaseQuery()
        {
            return DBContext.Games;
        }

        protected override Task<Game> GetItem(int id)
        {
            return GetBaseQuery().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}