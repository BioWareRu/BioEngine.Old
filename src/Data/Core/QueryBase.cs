using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.DB;

namespace BioEngine.Data.Core
{
    abstract class QueryBase<TOut> : MediatR.IRequest<TOut>
    {
        protected readonly BWContext DBContext;

        public QueryBase(BWContext dbContext)
        {
            DBContext = dbContext;
        }

        public abstract TOut Run();
    }
}