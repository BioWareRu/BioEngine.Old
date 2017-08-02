using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.DB;
using MediatR;

namespace BioEngine.Data.Core
{
    public abstract class HandlerBase
    {
        protected readonly BWContext DBContext;
        protected readonly IMediator Mediator;

        protected HandlerBase(IMediator mediator, BWContext dbContext)
        {
            DBContext = dbContext;
            Mediator = mediator;
        }
    }
}
