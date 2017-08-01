using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.API.Components.REST.Errors;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.API.Components.REST
{
    [Authorize(ActiveAuthenticationSchemes = "tokenAuth")]
    [Route("api/[controller]")]
    public abstract class RestController<T, TPkType> : Controller where T : BaseModel<TPkType>
    {
        protected readonly BWContext DBContext;

        protected RestController(BWContext dbContext)
        {
            DBContext = dbContext;
        }

        protected abstract IQueryable<T> GetBaseQuery();
        protected abstract Task<T> GetItem(TPkType id);

        [HttpGet]
        public virtual async Task<IActionResult> Get(QueryParams queryParams)
        {
            var query = GetBaseQuery();
            return Ok(new ListResult<T>(await query.ApplyParams(queryParams).ToListAsync(),
                await query.CountAsync()));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TPkType id)
        {
            var item = await GetItem(id);
            if (item == null)
            {
                return NotFound(new NotFoundError());
            }
            return Ok(item);
        }

        [HttpPost]
        [ValidateModel]
        public virtual async Task<IActionResult> Post([FromBody] T model)
        {
            return Ok(new SaveModelReponse<T>(StatusCodes.Status201Created, model));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public virtual async Task<IActionResult> Put(TPkType id, [FromBody] T model)
        {
            return Ok(new SaveModelReponse<T>(StatusCodes.Status202Accepted, model));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TPkType id)
        {
            throw new NotImplementedException();
        }
    }
}