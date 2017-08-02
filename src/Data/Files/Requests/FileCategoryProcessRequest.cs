using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class FileCategoryProcessRequest : CategoryProcessRequestBase<FileCat>
    {
        public FileCategoryProcessRequest(FileCat cat, ICategoryRequest<FileCat> request) : base(cat, request)
        {
        }
    }
}