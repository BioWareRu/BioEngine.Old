using System.Collections.Generic;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Queries
{
    public class GetNewsQuery : QueryBase<(IEnumerable<Common.Models.News> news, int count)>
    {
        public bool WithUnPublishedNews { get; }
        public int? Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public long? DateStart { get; }
        public long? DateEnd { get; }

        public GetNewsQuery(bool withUnPublishedNews = false, int? page = null, IParentModel parent = null,
            long? dateStart = null, long? dateEnd = null)
        {
            WithUnPublishedNews = withUnPublishedNews;
            Page = page;
            Parent = parent;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}