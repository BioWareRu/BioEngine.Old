﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Components.REST.Errors;
using BioEngine.API.Components.REST.Models;
using BioEngine.Common.Base;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Components.REST
{
    [Authorize(ActiveAuthenticationSchemes = "tokenAuth")]
    [Route("api/[controller]")]
    public abstract class RestController<T, TPkType> : Controller where T : BaseModel<TPkType>
    {
        protected readonly IMediator Mediator;

        protected RestController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected abstract Task<T> GetItem(TPkType id);

        protected abstract Task<(IEnumerable<T> items, int itemsCount)> GetItems(QueryParams queryParams);
        //protected abstract Task<T> UpdateItem(TPkType id, T model);
        //protected abstract Task<T> CreateItem<TCommand>(TCommand command) where TCommand : CommandWithResponseBase<T>;
        //protected abstract Task<T> DeleteItem(TPkType id);

        protected virtual async Task<T> CreateItem(CommandWithResponseBase<T> model)
        {
            return await Mediator.Send(model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get(QueryParams queryParams)
        {
            var itemsResult = await GetItems(queryParams);
            return Ok(new ListResult<T>(itemsResult.items, itemsResult.itemsCount));
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

        

        /*[HttpGet]
        public virtual async Task<IActionResult> Get(QueryParams queryParams)
        {
            var itemsResult = await GetItems(queryParams);
            return Ok(new ListResult<T>(itemsResult.items, itemsResult.itemsCount));
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
            await CreateItem(model);
            return Ok(new SaveModelReponse<T>(StatusCodes.Status201Created, model));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public virtual async Task<IActionResult> Put(TPkType id, [FromBody] T model)
        {
            var item = await GetItem(id);
            if (item == null)
            {
                return NotFound(new NotFoundError());
            }
            await UpdateItem(id, model);
            return Ok(new SaveModelReponse<T>(StatusCodes.Status202Accepted, model));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TPkType id)
        {
            await DeleteItem(id);
            return Ok("Deleted");
        }*/
    }
}