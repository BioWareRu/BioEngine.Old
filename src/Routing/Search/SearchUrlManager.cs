using System;
using BioEngine.Common.Base;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Search
{
    public class SearchUrlManager : UrlManager<SearchRoutesEnum>
    {
        public SearchUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper, appSettings)
        {
        }

        public Uri BlockUrl(string block, string term)
        {
            return GetUrl(SearchRoutesEnum.BlockPage, new {term, block});
        }
    }
}