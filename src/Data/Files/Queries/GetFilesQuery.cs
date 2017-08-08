using System;
using System.Linq;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetFilesQuery : ModelsListQueryBase<File>
    {
        public IParentModel Parent { get; set; }
        public override Func<IQueryable<File>, IQueryable<File>> OrderByFunc { get; protected set; } =
            query => query.OrderByDescending(x => x.Id);
    }
}