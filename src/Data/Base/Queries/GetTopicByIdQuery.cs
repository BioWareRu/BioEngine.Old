using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Queries
{
    public class GetTopicByIdQuery : SingleModelQueryBase<Topic>
    {
        public int Id { get; }

        public GetTopicByIdQuery(int id)
        {
            Id = id;
        }
    }
}