using System;
using System.Linq;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetCategoryFilesQuery : ModelsListQueryBase<File>
    {
        public FileCat Cat { get; }
        public override Func<IQueryable<File>, IQueryable<File>> OrderByFunc { get; protected set; }  =
            query => query.OrderByDescending(x => x.Id);

        public GetCategoryFilesQuery(FileCat cat)
        {
            Cat = cat;
        }
    }
}