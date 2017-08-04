using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class FileCategoryProcessQuery : CategoryProcessQueryBase<FileCat>
    {
        public FileCategoryProcessQuery(FileCat cat, ICategoryQuery<FileCat> query) : base(cat, query)
        {
        }
    }
}