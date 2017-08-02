using System;
using BioEngine.Common.Interfaces;

namespace BioEngine.Data.Core
{
    public abstract class CategoryProcessRequestBase<TCat> : RequestBase<TCat>
        where TCat : class, ICat
    {
        public TCat Cat { get; }

        public ICategoryRequest<TCat> CategoryRequest { get; }

        protected CategoryProcessRequestBase(TCat cat, ICategoryRequest<TCat> request)
        {
            Cat = cat;
            CategoryRequest = request;
        }
    }
}