using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Requests
{
    public class GetNewsRequest : RequestBase<(IEnumerable<Common.Models.News> news, int count)>
    {
        public bool WithUnPublishedNews { get; }
        public int? Page { get; }
        public IParentModel Parent { get; }

        public int PageSize { get; set; } = 20;

        public long? DateStart { get; }
        public long? DateEnd { get; }

        public GetNewsRequest(bool withUnPublishedNews = false, int? page = null, IParentModel parent = null,
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