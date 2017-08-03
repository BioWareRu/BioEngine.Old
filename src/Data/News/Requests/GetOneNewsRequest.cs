using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Requests
{
    public class GetOneNewsRequest : RequestBase<Common.Models.News>
    {
        public bool WithUnPublishedNews { get; }
        public long? DateStart { get; }
        public long? DateEnd { get; }

        public string Url { get; }

        public GetOneNewsRequest(string url, bool withUnPublishedNews = false, long? dateStart = null, long? dateEnd = null)
        {
            Url = url;
            WithUnPublishedNews = withUnPublishedNews;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
