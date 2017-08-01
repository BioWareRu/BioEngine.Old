using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components.REST;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.API.Controllers
{
    public class DevelopersController : RestController<Developer, int>
    {
        public DevelopersController(BWContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Developer> GetBaseQuery()
        {
            return DBContext.Developers;
        }

        protected override Task<Developer> GetItem(int id)
        {
            return GetBaseQuery().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}