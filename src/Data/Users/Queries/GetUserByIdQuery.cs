using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Users.Queries
{
    public class GetUserByIdQuery : QueryBase<User>
    {
        public int Id { get; }

        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
