using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Articles.Requests
{
    public class GetArticleByIdRequest : RequestBase<Article>
    {
        public GetArticleByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}