﻿using System.Threading.Tasks;
using AutoMapper;
using BioEngine.API.Auth;
using BioEngine.API.Components;
using BioEngine.API.Components.REST;
using BioEngine.API.Models.Files;
using BioEngine.Common.Models;
using BioEngine.Data.Files.Commands;
using BioEngine.Data.Files.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Controllers
{
    public class FilesCatsController : RestController<FileCat, int>
    {
        public FilesCatsController(RestContext<FilesCatsController> context) : base(context)
        {
        }

        [HttpGet]
        [UserRightsAuthorize(UserRights.Files)]
        public override async Task<IActionResult> Get(QueryParams queryParams)
        {
            var result =
                await Mediator.Send(new GetFilesCategoriesQuery {LoadChildren = false, LoadLastItems = null}
                    .SetQueryParams(queryParams));
            return List(result);
        }

        [HttpGet("{id}")]
        [UserRightsAuthorize(UserRights.Files)]
        public override async Task<IActionResult> Get(int id)
        {
            var fileCat = await GetFileCatById(id);
            return Model(fileCat);
        }

        private async Task<FileCat> GetFileCatById(int id)
        {
            return await Mediator.Send(new GetFileCategoryByIdQuery(id));
        }

        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [UserRightsAuthorize(UserRights.AddFiles)]
        public async Task<IActionResult> Post([FromBody] FileCatFormModel model, [FromServices] IMapper mapper)
        {
            var command = new CreateFileCatCommand();
            mapper.Map(model, command);
            var fileCatId = await Mediator.Send(command);
            return Created(await GetFileCatById(fileCatId));
        }
    }
}