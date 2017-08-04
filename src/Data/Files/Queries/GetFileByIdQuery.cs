using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Queries
{
    public class GetFileByIdQuery : QueryBase<File>
    {
        public GetFileByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}