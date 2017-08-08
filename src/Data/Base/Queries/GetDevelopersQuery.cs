using System;
using System.Linq;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetDevelopersQuery : ModelsListQueryBase<Developer>
    {
        public override Func<IQueryable<Developer>, IQueryable<Developer>> OrderByFunc { get; protected set; } =
            query => query.OrderBy(x => x.Name);
    }
}