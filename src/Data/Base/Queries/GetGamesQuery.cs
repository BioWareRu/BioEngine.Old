using System;
using System.Linq;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetGamesQuery : ModelsListQueryBase<Game>
    {
        public override Func<IQueryable<Game>, IQueryable<Game>> OrderByFunc { get; protected set; } =
            query => query.OrderBy(x => x.Title);
    }
}