using System;
using System.Linq;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetTopicsQuery : ModelsListQueryBase<Topic>
    {
        public override Func<IQueryable<Topic>, IQueryable<Topic>> OrderByFunc { get; protected set; } =
            query => query.OrderBy(x => x.Title);
    }
}