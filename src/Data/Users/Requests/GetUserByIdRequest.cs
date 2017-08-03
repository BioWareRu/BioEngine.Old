using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Users.Requests
{
    public class GetUserByIdRequest : RequestBase<User>
    {
        public int Id { get; }

        public GetUserByIdRequest(int id)
        {
            Id = id;
        }
    }
}
