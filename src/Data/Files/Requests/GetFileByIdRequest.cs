using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Files.Requests
{
    public class GetFileByIdRequest : RequestBase<File>
    {
        public GetFileByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}