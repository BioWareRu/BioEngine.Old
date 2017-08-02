using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Base.Requests
{
    public class GetGameByUrlRequest : RequestBase<Game>
    {
        public GetGameByUrlRequest(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}