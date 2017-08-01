using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Errors;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.API.Controllers
{
    public class TopicsController : RestController<Topic, int>
    {
        public TopicsController(BWContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Topic> GetBaseQuery()
        {
            return DBContext.Topics;
        }

        protected override Task<Topic> GetItem(int id)
        {
            return GetBaseQuery().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var query = DBContext.Topics;
            return Ok(new ListResult<Topic>(await query.ApplyParams(queryParams).ToListAsync(),
                await query.CountAsync()));
        }

        public override async Task<IActionResult> Get(int id)
        {
            var topic = await DBContext.Topics.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (topic == null)
            {
                return NotFound(new NotFoundError("Topic not found"));
            }
            return Ok(topic);
        }

        public override Task<IActionResult> Post(Topic model)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Put(int id, Topic model)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}