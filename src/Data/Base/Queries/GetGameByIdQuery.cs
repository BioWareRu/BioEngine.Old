using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetGameByIdQuery : SingleModelQueryBase<Game>
    {
        public int Id { get; }

        public GetGameByIdQuery(int id)
        {
            Id = id;
        }
    }
}