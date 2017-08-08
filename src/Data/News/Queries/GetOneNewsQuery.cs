using BioEngine.Data.Core;

namespace BioEngine.Data.News.Queries
{
    public class GetOneNewsQuery : SingleModelQueryBase<Common.Models.News>
    {
        public bool WithUnPublishedNews { get; set; }
        public long? DateStart { get; set; }
        public long? DateEnd { get; set; }

        public string Url { get; }

        public GetOneNewsQuery(string url)
        {
            Url = url;
        }
    }
}