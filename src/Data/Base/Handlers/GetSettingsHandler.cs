﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Base.Handlers
{
    public class GetSettingsHandler : RequestHandlerBase<GetSettingsRequest, List<Settings>>
    {
        public GetSettingsHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<List<Settings>> Handle(GetSettingsRequest message)
        {
            return await DBContext.Settings.ToListAsync();
        }
    }
}