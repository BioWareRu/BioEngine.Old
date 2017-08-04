using BioEngine.Data.Core;

namespace BioEngine.Data.News.Queries
{
    public class GetNewsByIdQuery : QueryBase<Common.Models.News>
    {
        public GetNewsByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}