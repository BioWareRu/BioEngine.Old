using BioEngine.Data.Core;

namespace BioEngine.Data.News.Queries
{
    public class GetOneNewsQuery : QueryBase<Common.Models.News>
    {
        public bool WithUnPublishedNews { get; }
        public long? DateStart { get; }
        public long? DateEnd { get; }

        public string Url { get; }

        public GetOneNewsQuery(string url, bool withUnPublishedNews = false, long? dateStart = null, long? dateEnd = null)
        {
            Url = url;
            WithUnPublishedNews = withUnPublishedNews;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
