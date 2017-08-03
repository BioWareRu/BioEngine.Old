using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Polls.Requests
{
    public class GetPollByIdRequest : RequestBase<Poll>
    {
        public GetPollByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
