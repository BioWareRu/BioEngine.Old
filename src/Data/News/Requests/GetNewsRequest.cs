using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Requests
{
    public class GetNewsRequest : RequestBase<(IEnumerable<Common.Models.News> news, int count)>
    {
        public bool WithUnPublishedNews { get; }
        public int Page { get; }

        public GetNewsRequest(bool withUnPublishedNews = false, int page = 1)
        {
            WithUnPublishedNews = withUnPublishedNews;
            Page = page;
        }
    }
}