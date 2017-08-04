using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Polls.Queries
{
    public class GetPollByIdQuery : QueryBase<Poll>
    {
        public GetPollByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
