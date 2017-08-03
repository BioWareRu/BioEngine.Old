using System;
using System.Collections.Generic;
using System.Text;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Requests
{
    public class GetNewsByIdRequest : RequestBase<Common.Models.News>
    {
        public GetNewsByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}